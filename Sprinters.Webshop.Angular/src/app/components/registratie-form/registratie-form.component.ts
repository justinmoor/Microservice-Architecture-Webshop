import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Klant } from '../../models/Klant';
import { environment } from '../../../environments/environment';
import { RegistratieService } from '../../services/registratie-service/registratie-service.service';
import {  MustMatch } from '../../helpers/wachtwoord-match';
import { LoginService } from '../../services/login-service/login.service';
import { Credentials } from '../../models/credentials';

@Component({
    selector: 'app-registratie-form',
    templateUrl: './registratie-form.component.html',
    styleUrls: ['./registratie-form.component.scss']
})
export class RegistratieFormComponent implements OnInit {

    error: string;
    registratieForm: FormGroup;

    constructor(private fb: FormBuilder,
                private router: Router,
                private registratieService: RegistratieService,
                private loginService: LoginService) {
    }

    ngOnInit() {
        const namePattern = '^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.\'-]+$';
        const phonePattern = '^((\\+|00?)31(\\s|\\s?\\-\\s?)?(\\(0\\)[\\-\\s]?)?|0)[1-9]((\\s|\\s?\\-\\s?)?[0-9])((\\s|\\s?-\\s?)?[0-9])((\\s|\\s?-\\s?)?[0-9])\\s?[0-9]\\s?[0-9]\\s?[0-9]\\s?[0-9]\\s?[0-9]$';

        this.registratieForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            wachtwoord: ['', [Validators.required, Validators.minLength(6)]],
            wachtwoord_herhaal: ['', [Validators.required, Validators.minLength(6)]],
            voornaam: ['', [Validators.pattern(namePattern), Validators.minLength(1)]],
            achternaam: ['', [Validators.required, Validators.minLength(2), Validators.pattern(namePattern)]],
            telefoon: ['', [Validators.required, Validators.pattern(phonePattern)]],
            adresregel: ['', [Validators.required, Validators.minLength(3)]],
            plaats: ['', [Validators.required, Validators.minLength(3)]],
            postcode: ['', [Validators.required, Validators.pattern('^[1-9][0-9]{3} ?(?!sa|SA|sd|SD|ss|SS)[a-zA-Z]{2}$')]]
        }, {
            validator: MustMatch('wachtwoord', 'wachtwoord_herhaal')
        });
    }

    get getFormControls() { return this.registratieForm.controls; }

    veldInvalid(veld: string) {
        return this.registratieForm.get(veld).touched && this.registratieForm.get(veld).invalid;
    }

    registreer() {
        const postcodeTrimmed = this.registratieForm.get('postcode').value.replace(' ', '');
        this.registratieForm.get('postcode').setValue(postcodeTrimmed);

        const klant: Klant = {
            voornaam:           this.registratieForm.get('voornaam').value,
            achternaam:         this.registratieForm.get('achternaam').value,
            telefoonnummer:     this.registratieForm.get('telefoon').value,
            adresRegel:         this.registratieForm.get('adresregel').value,
            plaats:             this.registratieForm.get('plaats').value,
            postcode:           this.registratieForm.get('postcode').value,
            email:              this.registratieForm.get('email').value,
            wachtwoord:         this.registratieForm.get('wachtwoord').value
        };

        this.registratieService.registreer(klant).subscribe(
            () => {
                sessionStorage.setItem('message', 'Account succesvol aangemaakt');
                this.logKlantDirectIn(klant);
            },
            error => {
                if (!environment.production) { console.log(error); }
                switch (error.status) {
                    case 409:
                    case 412:
                    case 503:
                    case 500: {
                        this.error = error.error;
                        break;
                    }
                    default: {
                        this.error = 'Er is iets fout gegaan, probeer het later opnieuw';
                        break;
                    }
                }
                return;
            }
        );
    }

    private logKlantDirectIn(klant: Klant) {
        const creds: Credentials = {
            userName: klant.email,
            password: klant.wachtwoord
        };

        this.loginService.logIn(creds).subscribe(_ => {
            this.router.navigate(['/']);
        }, error => {
            if (!environment.production) { console.log(error); }
            this.router.navigate(['/inloggen']);
        });
    }
}
