import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { ContratosModificacionesContractualesService } from '../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { Contrato } from '../../../../_interfaces/contratos-modificaciones.interface';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-registro-tramite',
  templateUrl: './form-registro-tramite.component.html',
  styleUrls: ['./form-registro-tramite.component.scss']
})
export class FormRegistroTramiteComponent implements OnInit, OnDestroy {

  archivo: string;
  seRealizoPeticion = false;
  modalidadContratoArray: Dominio[] = [];
  @Input() dataFormulario: FormGroup;
  @Input() contratoId: number;
  @Input() contratacionId: number;
  @Input() fechaTramite: Date;
  @Input() estaEditando: boolean;
  estadoCodigo: string;
  estadoCodigos = {
    enRevision: '2',
    enFirmaFiduciaria: '5',
    enFirmaContratista: '10',
    firmado: '8'
  };
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(
    private dialog: MatDialog,
    private routes: Router,
    private contratosContractualesSvc: ContratosModificacionesContractualesService,
    private commonSvc: CommonService )
  {
    this.commonSvc.modalidadesContrato()
      .subscribe( response => this.modalidadContratoArray = response );
  }

  ngOnDestroy(): void {
    if ( this.dataFormulario.dirty === true && this.seRealizoPeticion === false ) {
      this.openDialogConfirmar( '', '¿Desea guardar la información registrada?' );
    }
  }

  ngOnInit(): void {
    this.archivo = this.dataFormulario.get( 'documento' ).value;
  }

  campoSinDiligenciar( campoFormulario: string ) {
    if ( campoFormulario === 'numeroContrato' && this.dataFormulario.get( campoFormulario ).value.length === 0 ) {
      this.dataFormulario.get( campoFormulario ).markAsTouched();
    }
    if ( this.dataFormulario.get( campoFormulario ).value === null ) {
      this.dataFormulario.get( campoFormulario ).markAsTouched();
    }
  }

  fileName( event: any ) {

    if ( event.target.files.length > 0) {
      const file   = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.dataFormulario.patchValue({
        documentoFile: file
      });
    }

  }

  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe( response => {
        if ( response === true ) {
          this.guardar();
        }
      } );
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  textoLimpioMessage(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  guardar() {
    this.estaEditando = true;
    if ( this.dataFormulario.get( 'numeroContrato' ).value.length === 0 ) {
      this.openDialog( '', '<b>Falta registrar información</b>.' );
      this.dataFormulario.get( 'numeroContrato' ).markAsTouched();
      return;
    }

    const pContrato = new FormData();
    pContrato.append( 'contratacionId', `${ this.contratacionId }` );

    if ( this.contratoId !== null ) {
      pContrato.append( 'contratoId', `${ this.contratoId }` );
    }

    if ( this.dataFormulario.get( 'numeroContrato' ).value !== null ) {
      pContrato.append( 'numeroContrato', `${ this.dataFormulario.get( 'numeroContrato' ).value }` );
    }

    if ( this.dataFormulario.get( 'modalidadContrato' ).value !== null ) {
      pContrato.append( 'modalidadCodigo', this.dataFormulario.get( 'modalidadContrato' ).value );
      this.estadoCodigo = this.estadoCodigos.enRevision;
    }

    if ( this.dataFormulario.get( 'fechaEnvioParaFirmaContratista' ).value !== null ) {
      const fechaEnvioFirma = new Date( this.dataFormulario.get( 'fechaEnvioParaFirmaContratista' ).value );
      const fechaEnvioFirmas = fechaEnvioFirma.toISOString();
      pContrato.append( 'fechaEnvioFirma', `${ fechaEnvioFirmas }` );
      this.estadoCodigo = this.estadoCodigos.enFirmaContratista;
    }

    if ( this.dataFormulario.get( 'fechaFirmaPorParteContratista' ).value !== null ) {
      const fechaFirmaContratista = new Date( this.dataFormulario.get( 'fechaFirmaPorParteContratista' ).value );
      const fechaFirmaContratistas = fechaFirmaContratista.toISOString();
      pContrato.append( 'fechaFirmaContratista', `${ fechaFirmaContratistas }` );
    } else {
      pContrato.append( 'fechaFirmaContratista', null );
    }

    if ( this.dataFormulario.get( 'fechaEnvioParaFirmaFiduciaria' ).value !== null ) {
      const fechaFirmaFiduciaria = new Date( this.dataFormulario.get( 'fechaEnvioParaFirmaFiduciaria' ).value );
      const fechaFirmaFiduciarias = fechaFirmaFiduciaria.toISOString();
      pContrato.append( 'fechaFirmaFiduciaria', `${ fechaFirmaFiduciarias }` );
      this.estadoCodigo = this.estadoCodigos.enFirmaFiduciaria;
    } else {
      pContrato.append( 'fechaFirmaFiduciaria', null );
    }

    if ( this.dataFormulario.get( 'fechaFirmaPorParteFiduciaria' ).value !== null ) {
      const fechaFirmaContrato = new Date( this.dataFormulario.get( 'fechaFirmaPorParteFiduciaria' ).value );
      const fechaFirmaContratos = fechaFirmaContrato.toISOString();
      pContrato.append( 'fechaFirmaContrato', `${ fechaFirmaContratos }` );
      this.estadoCodigo = this.estadoCodigos.enRevision;
    } else {
      pContrato.append( 'fechaFirmaContrato', null );
    }

    if ( this.dataFormulario.get( 'observaciones' ).value ) {
      pContrato.append( 'observaciones', `${ this.dataFormulario.get( 'observaciones' ).value }` );
    }

    if ( this.dataFormulario.get( 'rutaDocumento' ).value !== null ) {
      pContrato.append( 'rutaDocumento', this.dataFormulario.get( 'rutaDocumento' ).value );
    }

    if (this.dataFormulario.valid) this.estadoCodigo = this.estadoCodigos.firmado;
    this.contratosContractualesSvc.postRegistroTramiteContrato( pContrato, this.estadoCodigo )
      .subscribe(
        ( resp: any ) => {
          this.seRealizoPeticion = true;
          this.openDialog( '', `<b>${resp.message}</b>` );
          this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
        },
        ( error: any ) => {
          this.openDialog( '', `<b>${error.message}</b>` );
        }
      );

  }

}
