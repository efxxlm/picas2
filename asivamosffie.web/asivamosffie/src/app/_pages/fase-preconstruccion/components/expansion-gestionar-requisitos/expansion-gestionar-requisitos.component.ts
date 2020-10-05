import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-expansion-gestionar-requisitos',
  templateUrl: './expansion-gestionar-requisitos.component.html',
  styleUrls: ['./expansion-gestionar-requisitos.component.scss']
})
export class ExpansionGestionarRequisitosComponent implements OnInit {

  constructor() {

  }

  ngOnInit(): void {
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

}
