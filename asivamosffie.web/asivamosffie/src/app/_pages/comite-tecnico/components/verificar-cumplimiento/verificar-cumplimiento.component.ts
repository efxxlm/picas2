import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';

@Component({
  selector: 'app-verificar-cumplimiento',
  templateUrl: './verificar-cumplimiento.component.html',
  styleUrls: ['./verificar-cumplimiento.component.scss']
})
export class VerificarCumplimientoComponent implements OnInit {

  addressForm = this.fb.group({
    proceso: this.fb.array([])
  });

  procesosArray = ['Sin iniciar', 'En proceso', 'Finalizada']

  get proceso() {
    return this.addressForm.get('proceso') as FormArray;
  }

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  cargarDatos() {
    const aprobacion: any[] = ['', '', ''];

    aprobacion.forEach(valor => this.proceso.push(this.fb.control(valor)));
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }


}
