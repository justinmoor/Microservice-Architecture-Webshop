import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { WinkelmandItem } from '../../models/winkelmand-item';
import { Klant } from '../../models/Klant';
import { BestelService } from '../../services/bestel-service/bestel.service';
import { Router } from '@angular/router';
import { UserService } from '../../services/user-service/user.service';
import { Bestelling } from '../../models/bestelling';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-adres-form',
  templateUrl: './adres-form.component.html',
  styleUrls: ['./adres-form.component.scss']
})
export class AdresFormComponent implements OnInit {

  error: boolean;
  adresForm: FormGroup;
  items: WinkelmandItem[];
  klant: Klant;

  constructor(
    private fb: FormBuilder,
    private bestelService: BestelService,
    private router: Router,
    private userService: UserService
  ) {
  }

  ngOnInit() {
    this.adresForm = this.fb.group({
      adresregel1: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(255)]],
      adresregel2: ['', [Validators.maxLength(255)]],
      plaats: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(255)]],
      postcode: ['', [
        Validators.required,
        Validators.pattern('^[1-9][0-9]{3} ?(?!sa|SA|sd|SD|ss|SS)[a-zA-Z]{2}$'),
        Validators.maxLength(7)
      ]]
    });

    this.userService.getKlantGegevens().subscribe(klant => {
      this.klant = klant;
      this.adresForm.get('adresregel1').setValue(klant.adresRegel);
      // this.adresForm.get('adresregel2').setValue(klant.adresRegel2);
      this.adresForm.get('plaats').setValue(klant.plaats);
      this.adresForm.get('postcode').setValue(klant.postcode);
    });

    this.error = false;
    this.items = JSON.parse(localStorage.getItem('winkelmand'));
  }

  veldInvalid(veld: string) {
    return this.adresForm.get(veld).touched && this.adresForm.get(veld).invalid;
  }

  plaatsBestelling() {
    const postcodeTrimmed = this.adresForm.get('postcode').value.replace(' ', '');
    this.adresForm.get('postcode').setValue(postcodeTrimmed);

    const bestelling: Bestelling = {
      klantId: this.klant.id,
      adresregel1: this.adresForm.get('adresregel1').value,
      adresregel2: this.adresForm.get('adresregel2').value,
      plaats: this.adresForm.get('plaats').value,
      postcode: this.adresForm.get('postcode').value,
      besteldeArtikelen: this.items
    };

    this.bestelService.plaatsBestelling(bestelling).subscribe(
      response => {
        sessionStorage.setItem('besteld', JSON.stringify(response));
        this.router.navigate(['/bestellingsuccesvol']);
      },
      error => {
        if (!environment.production) { console.log(error); }
        this.error = true;
      }
    );
  }

  zijnWinkelmandItemsAanwezig() {
    return this.items !== null;
  }

}
