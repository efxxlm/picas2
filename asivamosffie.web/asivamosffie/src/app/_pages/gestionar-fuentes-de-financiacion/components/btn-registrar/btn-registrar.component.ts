import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-btn-registrar',
  templateUrl: './btn-registrar.component.html',
  styleUrls: ['./btn-registrar.component.scss']
})
export class BtnRegistrarComponent implements OnInit {

  verAyuda = false;

  tiposAportante = [
    { name: 'ET', value: 1 },
    { name: 'ETC', value: 2 },
    { name: 'FFIE', value: 3 },
    { name: 'Otro', value: 4 },
    { name: 'Terceros', value: 5 }
  ];

  regitrarAporteForm = this.fb.group({
    tipoAportante: [null, Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,) { }

  ngOnInit(): void {
  }

  onSubmit() {
    if (this.regitrarAporteForm.valid) {
      this.router.navigate(['./gestionarFuentes/registrar']);
    }
  }

}
