import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-definir-caracteristicas',
  templateUrl: './definir-caracteristicas.component.html',
  styleUrls: ['./definir-caracteristicas.component.scss']
})
export class DefinirCaracteristicasComponent implements OnInit {
  addressForm = this.fb.group({
    completada: null,
    reasignacion: null,
    avanceObra: null,
    porcentajeAvanceObra: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(3), Validators.min(1), Validators.max(100)])
    ],
    requiereLicencias: null,
    licenciaVigente: null,
    numeroLicencia: null,
    fechaVigencia: null
  });

  idSolicitud: number;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute
    ) {}

    ngOnInit(): void {
      this.route.params.subscribe((params: Params) => {
        console.log(params);
        const id = params.id;
        console.log(id);
        this.identificador(id);
      });
    }

    identificador(id: number) {
      this.idSolicitud = id;
    }

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
