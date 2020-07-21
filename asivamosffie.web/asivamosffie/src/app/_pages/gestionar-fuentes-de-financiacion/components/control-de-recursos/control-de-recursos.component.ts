import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { TiposAportante } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { FuenteFinanciacion, FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';

@Component({
  selector: 'app-control-de-recursos',
  templateUrl: './control-de-recursos.component.html',
  styleUrls: ['./control-de-recursos.component.scss']
})
export class ControlDeRecursosComponent implements OnInit {
  
  addressForm: FormGroup;
  hasUnitNumber = false;
  tipoAportante = TiposAportante;
  nombreAportante: string = '';
  departamento: string = '';
  municipio: string = '';
  vigencia: string = '';
  nombreFuente: string = '';
  valorFuente: number = 0;
  idFuente: number = 0;

  NombresDeLaCuenta = [{
      name: 'Recursos corrientes',
      value: 1
    }];

  rpArray = [{
      name: 'Recursos corrientes',
      value: 1
    }];

  constructor(
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private fuenteFinanciacionServices: FuenteFinanciacionService
             ) 
  {}

  ngOnInit(): void {
    this.addressForm = this.fb.group({
      nombreCuenta: [null, Validators.required],
      numeroCuenta: [null, Validators.required],
      rp: [null],
      vigencia: [null, Validators.required],
      fechaConsignacion: [null, Validators.required],
      valorConsignacion: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(50)])
      ],
    });

    this.activatedRoute.params.subscribe( param => {
      this.idFuente = param['id'];
      this.fuenteFinanciacionServices.getFuenteFinanciacion( this.idFuente ).subscribe( ff => {
        console.log( ff );
      }) 
    })

  }
}
