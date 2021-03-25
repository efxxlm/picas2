import { DOCUMENT } from '@angular/common';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
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
  modalidadContratoArray: Dominio[] = [];
  fechaTramite: Date = new Date();
  estaEditando = false;

  constructor ( private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private routes: Router,
                private commonSvc: CommonService,
                private dialog: MatDialog,
                private contratosContractualesSvc: ContratosModificacionesContractualesService,
                @Inject(DOCUMENT) readonly document: Document ) {
    this.crearFormulario();
    this.getContratacionId( this.activatedRoute.snapshot.params.id );
    this.getEstadoCodigo();
  };

  ngOnInit(): void {
  };

  crearFormulario () {
    this.form = this.fb.group({
      numeroContrato                : [ '', Validators.required ],
      modalidadContrato             : [ null, Validators.required ],
      fechaEnvioParaFirmaContratista: [ null, Validators.required ],
      fechaFirmaPorParteContratista : [ null, Validators.required ],
      fechaEnvioParaFirmaFiduciaria : [ null, Validators.required ],
      fechaFirmaPorParteFiduciaria  : [ null, Validators.required ],
      observaciones                 : [ null, Validators.required ],
      documento                     : [ null, Validators.required ],
      rutaDocumento                 : [ null, Validators.required ],
      documentoFile                 : [ null, Validators.required ]
    });
  };

  get window(): Window { return this.document.defaultView; }

  goToLink(url: string){
    if ( url.includes("http://") || url.includes("https://"))
      this.window.open(url, "_blank");
    else{
      window.open('//' + url, "_blank");
    }
  }

  getContratacionId ( id ) {
    this.contratosContractualesSvc.getContratacionId( id )
      .subscribe( ( resp: any ) => {
        this.commonSvc.modalidadesContrato()
        .subscribe( modalidadContrato => {
          this.contratacion = resp;
          console.log( this.contratacion );
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
            this.modalidadContratoArray = modalidadContrato;
            this.form.reset({
              numeroContrato: resp.contrato[0].numeroContrato || '',
              modalidadContrato: resp.contrato[0].modalidadCodigo !== undefined ? modalidadContrato.filter( modalidad => modalidad.codigo === resp.contrato[0].modalidadCodigo )[0].codigo : null,
              fechaEnvioParaFirmaContratista: resp.contrato[0].fechaEnvioFirma || null,
              fechaFirmaPorParteContratista: resp.contrato[0].fechaFirmaContratista || null,
              fechaEnvioParaFirmaFiduciaria: resp.contrato[0].fechaFirmaFiduciaria || null,
              fechaFirmaPorParteFiduciaria: resp.contrato[0].fechaFirmaContrato || null,
              observaciones: resp.contrato[0].observaciones || null,
              documento: rutaDocumento,
              rutaDocumento: resp.contrato[0].rutaDocumento !== undefined ? resp.contrato[0].rutaDocumento : null
            });
            console.log( this.form.value );
            this.estaEditando = true;
          };
        } );
      } );
  };

  getModalidadContrato( modalidadCodigo: string ) {
    if ( this.modalidadContratoArray.length > 0 ) {
        const modalidad = this.modalidadContratoArray.find( modalidad => modalidad.codigo === modalidadCodigo );

        if ( modalidad !== undefined ) {
          return modalidad.nombre;
        }
    }
  }

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
        () => this.openDialog( '', `<b>Archivo no encontrado.</b>` )
      );
  };

};
