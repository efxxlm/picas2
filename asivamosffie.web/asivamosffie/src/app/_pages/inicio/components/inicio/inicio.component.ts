import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.scss']
})
export class InicioComponent implements OnInit {

  emailField: FormControl;
  verClave = true;

  constructor() {
    this.emailField = new FormControl('', [Validators.required, Validators.email]);
  }

  ngOnInit(): void {

  }

}
