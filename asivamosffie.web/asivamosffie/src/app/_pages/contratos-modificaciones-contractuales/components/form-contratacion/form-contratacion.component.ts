import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ContratosModificacionesContractualesService } from '../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';

@Component({
  selector: 'app-form-contratacion',
  templateUrl: './form-contratacion.component.html',
  styleUrls: ['./form-contratacion.component.scss']
})
export class FormContratacionComponent implements OnInit {

  form: FormGroup;
  estadoCodigo: string;
  estadoCodigos = {
    enRevision: '9',
    enFirmaFiduciaria: '11'
  }
  contratacion: any;

  constructor ( private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService ) {
    this.getContratacionId( this.activatedRoute.snapshot.params.id );
    this.getEstadoCodigo();
    this.crearFormulario();
  };

  ngOnInit(): void {
  };

  getContratacionId ( id ) {
    console.log( id );
    this.contratosContractualesSvc.getContratacionId( id )
      .subscribe( ( resp: any ) => {
        this.contratacion = resp;
        console.log( this.contratacion );
      } );
  };

  getEstadoCodigo () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl || this.routes.getCurrentNavigation().extras.skipLocationChange === false ) {
      this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
      return;
    }
    
    if ( this.routes.getCurrentNavigation().extras.state.estadoCodigo === this.estadoCodigos.enRevision ) {
      this.estadoCodigo = this.estadoCodigos.enFirmaFiduciaria;
    }
    
  }

  crearFormulario () {
    this.form = this.fb.group({
      numeroContrato                : [ '', Validators.required ],
      fechaEnvioParaFirmaContratista: [ null ],
      fechaFirmaPorParteContratista : [ null ],
      fechaEnvioParaFirmaFiduciaria : [ null ],
      fechaFirmaPorParteFiduciaria  : [ null ],
      observaciones                 : [ null ],
      documento                     : [ null ],
      documentoFile                 : [ null ]
    });
  };



};
