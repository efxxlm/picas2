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
    enRevision: '2',
    enFirmaFiduciaria: '5',
    firmado: '6'
  }
  contratacion: any;
  fechaTramite: Date = new Date();

  constructor ( private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService ) {
    this.crearFormulario();
    this.getContratacionId( this.activatedRoute.snapshot.params.id );
    this.getEstadoCodigo();
  };

  ngOnInit(): void {
  };

  crearFormulario () {
    this.form = this.fb.group({
      numeroContrato                : [ '', Validators.required ],
      fechaEnvioParaFirmaContratista: [ null ],
      fechaFirmaPorParteContratista : [ null ],
      fechaEnvioParaFirmaFiduciaria : [ null ],
      fechaFirmaPorParteFiduciaria  : [ null ],
      observaciones                 : [ null ],
      documento                     : [ null ],
      rutaDocumento                 : [ null ],
      documentoFile                 : [ null ]
    });
  };

  getContratacionId ( id ) {
    this.contratosContractualesSvc.getContratacionId( id )
      .subscribe( ( resp: any ) => {
        this.contratacion = resp;
        if ( resp.contrato.length > 0 ) {
          this.form.reset({
            numeroContrato: resp.contrato[0].numeroContrato || '',
            fechaEnvioParaFirmaContratista: resp.contrato[0].fechaEnvioFirma || null,
            fechaFirmaPorParteContratista: resp.contrato[0].fechaFirmaContratista || null,
            fechaEnvioParaFirmaFiduciaria: resp.contrato[0].fechaFirmaFiduciaria || null,
            fechaFirmaPorParteFiduciaria: resp.contrato[0].fechaFirmaContrato || null,
            observaciones: this.textoLimpioMessage( resp.contrato[0].observaciones ) || null,
            rutaDocumento: resp.contrato[0].rutaDocumento || null
          });
        };
      } );
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml;
  }

  getEstadoCodigo () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl || this.routes.getCurrentNavigation().extras.skipLocationChange === false ) {
      this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
      return;
    }
    
    this.estadoCodigo = this.routes.getCurrentNavigation().extras.state.estadoCodigo;
    
  };

};
