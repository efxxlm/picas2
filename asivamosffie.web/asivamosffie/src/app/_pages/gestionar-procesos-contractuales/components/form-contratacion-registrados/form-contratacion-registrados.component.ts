import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ContratosModificacionesContractualesService } from './../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-contratacion-registrados',
  templateUrl: './form-contratacion-registrados.component.html',
  styleUrls: ['./form-contratacion-registrados.component.scss']
})
export class FormContratacionRegistradosComponent implements OnInit {

  form: FormGroup;
  estadoCodigo: string;
  modalidadContratoArray: Dominio[] = [];
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
                private dialog: MatDialog,
                private commonSvc: CommonService,
                private contratosContractualesSvc: ContratosModificacionesContractualesService )
  {
    this.crearFormulario();
    this.getContratacionId( this.activatedRoute.snapshot.params.id );
    this.getEstadoCodigo();
    this.commonSvc.modalidadesContrato()
      .subscribe( response => this.modalidadContratoArray = response );
  };

  ngOnInit(): void {
    console.log( this.estadoCodigo );
  };

  crearFormulario () {
    this.form = this.fb.group({
      numeroContrato                : [ '', Validators.required ],
      modalidadContrato             : [ null ],
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

  getModalidadContrato( modalidadCodigo: string ) {
    if ( this.modalidadContratoArray.length > 0 ) {
      const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
      return modalidad[0].nombre;
    }
  }

  getContratacionId ( id ) {
    this.contratosContractualesSvc.getContratacionId( id )
      .subscribe( ( resp: any ) => {
        if ( resp.contrato.length === 0 ) {
          this.routes.navigate( [ '/procesosContractuales' ] );
        };
        this.contratacion = resp;
        console.log( resp );
        if ( resp.contrato.length > 0 ) {
          let rutaDocumento;
          if ( resp.contrato[0].rutaDocumento !== undefined ) {
            rutaDocumento = resp.contrato[0].rutaDocumento.split( /\\/gi );
            console.log( rutaDocumento );
            rutaDocumento = rutaDocumento[ rutaDocumento.length -1 ];
          } else {
            rutaDocumento = null;
          };
          console.log( resp.contrato[0] );
          this.form.reset({
            numeroContrato: resp.contrato[0].numeroContrato || '',
            modalidadContrato: resp.contrato[0].modalidadCodigo,
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

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  getEstadoCodigo () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl || this.routes.getCurrentNavigation().extras.skipLocationChange === false ) {
      this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
      return;
    }
    
    this.estadoCodigo = this.routes.getCurrentNavigation().extras.state.estadoCodigo;
    
  }

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
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
        () => this.openDialog( '', `<b>Archivo no encontrado.</b>` )
      );
  };

};
