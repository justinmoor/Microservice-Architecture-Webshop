import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BetalingService } from '../../services/betaling/betaling.service';
import { Betaling } from '../../models/betaling';

@Component({
  selector: 'betaling-invoeren',
  templateUrl: './betaling-invoeren.component.html',
  styleUrls: ['./betaling-invoeren.component.scss']
})
export class BetalingInvoerenComponent implements OnInit {

  succes: boolean;
  error: boolean;
  betalingInvoerenForm: FormGroup;

  constructor(private fb: FormBuilder, private betalingService: BetalingService){

  }

  ngOnInit() {
      this.betalingInvoerenForm = this.fb.group({
          factuurnummer:  ['', [Validators.required]],
          bedrag:         ['', [Validators.required, Validators.min(1)]]
      });
  }

  plaats(){
      let betaling: Betaling = {
          factuurnummer:  this.betalingInvoerenForm.get("factuurnummer").value,
          bedrag:         this.betalingInvoerenForm.get("bedrag").value
      }

      this.betalingService.plaats(betaling).subscribe(
          response => {
              this.betalingInvoerenForm.reset();
              this.succes = true;
              this.error = false;
          }, 
          error => {
              this.error = true;
              this.succes = false;
          }
      );
  }
}
