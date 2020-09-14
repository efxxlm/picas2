import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-consideraciones-especiales',
  templateUrl: './consideraciones-especiales.component.html',
  styleUrls: ['./consideraciones-especiales.component.scss']
})
export class ConsideracionesEspecialesComponent implements OnInit {

  addressForm = this.fb.group({
    reasignacion: ['free', Validators.required],
    descripcion: null
  });

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }

}
