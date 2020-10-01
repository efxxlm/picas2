import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-expansion-gestionar-interventoria',
  templateUrl: './expansion-gestionar-interventoria.component.html',
  styleUrls: ['./expansion-gestionar-interventoria.component.scss']
})
export class ExpansionGestionarInterventoriaComponent implements OnInit {

  estado: FormControl;
  cantidadPerfiles: FormControl;

  estadoProyectoArray = [
    {
      name: 'Estudios y Diseños',
      value: 1
    },
    {
      name: 'Diagnóstico',
      value: 2
    }
  ];


  constructor() {
    this.declararEstado();
    this.declararPerfiles();
    this.cantidadPerfiles.valueChanges
    .subscribe(value => {
      console.log(value);
    });
  }

  ngOnInit(): void {
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  private declararEstado() {
    this.cantidadPerfiles = new FormControl('', Validators.required);
  }

  private declararPerfiles() {
    this.cantidadPerfiles = new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(3),
      Validators.min(1),
      Validators.max(999)
    ]);
  }

}
