import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
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
                private commonSvc: CommonService,
                private dialog: MatDialog,
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
        console.log( this.contratacion );
        if ( resp.contrato.length > 0 ) {
          let rutaDocumento;
          if ( resp.contrato[0].rutaDocumento !== undefined ) {
            rutaDocumento = resp.contrato[0].rutaDocumento.split( /[^\w\s]/gi );
            rutaDocumento = `${ rutaDocumento[ rutaDocumento.length -2 ] }.${ rutaDocumento[ rutaDocumento.length -1 ] }`;
          } else {
            rutaDocumento = null;
          };
          this.form.reset({
            numeroContrato: resp.contrato[0].numeroContrato || '',
            fechaEnvioParaFirmaContratista: resp.contrato[0].fechaEnvioFirma || null,
            fechaFirmaPorParteContratista: resp.contrato[0].fechaFirmaContratista || null,
            fechaEnvioParaFirmaFiduciaria: resp.contrato[0].fechaFirmaFiduciaria || null,
            fechaFirmaPorParteFiduciaria: resp.contrato[0].fechaFirmaContrato || null,
            observaciones: resp.contrato[0].observaciones || null,
            documento: rutaDocumento,
            rutaDocumento: resp.contrato[0].rutaDocumento !== undefined ? resp.contrato[0].rutaDocumento : null
          });
          console.log( this.form.value );
        };
      } );
  };

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  innerObservacion ( observacion: string ) {
    if ( observacion !== null ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    };
  };

  getEstadoCodigo () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl || this.routes.getCurrentNavigation().extras.skipLocationChange === false ) {
      this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
      return;
    }
    
    this.estadoCodigo = this.routes.getCurrentNavigation().extras.state.estadoCodigo;
    
  };

  getDocumento ( nombreDocumento: string ) {
    this.commonSvc.getDocumento( nombreDocumento )
      .subscribe(
        response => {

          const documento = `Documento suscrito`;
          const text = documento,
          blob = new Blob([response], { type: 'application/pdf' }),
          anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = ['application/vnd.openxmlformats-officedocument.wordprocessingml.document', anchor.download, anchor.href].join(':');
          anchor.click();

        },
        err => this.openDialog( '', `<b>Archivo no encontrado.</b>` )
      );
  };

};
