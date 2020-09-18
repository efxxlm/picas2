import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { ContratosModificacionesContractualesService } from '../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { Contrato } from '../../../../_interfaces/contratos-modificaciones.interface';

@Component({
  selector: 'app-form-registro-tramite',
  templateUrl: './form-registro-tramite.component.html',
  styleUrls: ['./form-registro-tramite.component.scss']
})
export class FormRegistroTramiteComponent implements OnInit {

  archivo                : string;
  @Input() dataFormulario: FormGroup;
  @Input() contratoId    : number;
  @Input() contratacionId: number;
  estadoCodigo  : string;
  estadoCodigos = {
    enRevision: '9',
    enFirmaFiduciaria: '11',
    firmado: '12'
  }
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

  constructor ( private dialog: MatDialog,
                private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService ) { }

  ngOnInit(): void {
  };

  fileName ( event: any ) {
    
    if ( event.target.files.length > 0) {
      const file   = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.dataFormulario.patchValue({
        documentoFile: file
      });
    };

  };

  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  guardar () {
    console.log( this.dataFormulario );

    if ( this.dataFormulario.invalid ) {
      this.openDialog( 'Falta registrar información.', '' );
      return;
    };

    let pContrato = new FormData();
    pContrato.append( 'contratacionId', `${ this.contratacionId }` );
    pContrato.append( 'contratoId', `${ this.contratoId }` );
    if ( this.dataFormulario.get( 'numeroContrato' ).value !== null ) {
      pContrato.append( 'numeroContrato', `${ this.dataFormulario.get( 'numeroContrato' ).value }` );
      this.estadoCodigo = this.estadoCodigos.enRevision;
    }

    if ( this.dataFormulario.get( 'fechaEnvioParaFirmaContratista' ).value !== null ) {
      let fechaEnvioFirma = this.dataFormulario.get( 'fechaEnvioParaFirmaContratista' ).value;
      //fechaEnvioFirma = `${ fechaEnvioFirma.getFullYear() }/${ fechaEnvioFirma.getMonth() + 1 }/${ fechaEnvioFirma.getDate() }`;
      fechaEnvioFirma = fechaEnvioFirma.toISOString();
      pContrato.append( 'fechaEnvioFirma', `${ fechaEnvioFirma }` );
      this.estadoCodigo = this.estadoCodigos.enFirmaFiduciaria;
    }

    console.log( pContrato.get( 'fechaEnvioFirma' ) );

    if ( this.dataFormulario.get( 'fechaFirmaPorParteContratista' ).value !== null ) {
      let fechaFirmaContratista = this.dataFormulario.get( 'fechaFirmaPorParteContratista' ).value;
      fechaFirmaContratista = `${ fechaFirmaContratista.getFullYear() }/${ fechaFirmaContratista.getMonth() + 1 }/${ fechaFirmaContratista.getDate() }`;
      pContrato.append( 'fechaFirmaContratista', `${ fechaFirmaContratista }` );
    } else {
      pContrato.append( 'fechaFirmaContratista', null );
    }

    if ( this.dataFormulario.get( 'fechaEnvioParaFirmaFiduciaria' ).value !== null ) {
      let fechaFirmaFiduciaria = this.dataFormulario.get( 'fechaEnvioParaFirmaFiduciaria' ).value;
      fechaFirmaFiduciaria = `${ fechaFirmaFiduciaria.getFullYear() }/${ fechaFirmaFiduciaria.getMonth() + 1 }/${ fechaFirmaFiduciaria.getDate() }`; 
      pContrato.append( 'fechaFirmaFiduciaria', `${ fechaFirmaFiduciaria }` );
    } else {
      pContrato.append( 'fechaFirmaFiduciaria', null );
    }

    if ( this.dataFormulario.get( 'fechaFirmaPorParteFiduciaria' ).value !== null ) {
      let fechaFirmaContrato = this.dataFormulario.get( 'fechaFirmaPorParteFiduciaria' ).value;
      fechaFirmaContrato = `${ fechaFirmaContrato.getFullYear() }/${ fechaFirmaContrato.getMonth() + 1 }/${ fechaFirmaContrato.getDate() }`;
      pContrato.append( 'fechaFirmaContrato', `${ fechaFirmaContrato }` );
    } else {
      pContrato.append( 'fechaFirmaContrato', null );
    }

    if ( this.dataFormulario.get( 'observaciones' ).value ) {
      pContrato.append( 'observaciones', `${ this.dataFormulario.get( 'observaciones' ).value }` );
    }

    if ( this.dataFormulario.get( 'documentoFile' ).value !== null ) {
      pContrato.append( 'pFile', this.dataFormulario.get( 'documentoFile' ).value );
      this.estadoCodigo = this.estadoCodigos.firmado;
    }

    this.contratosContractualesSvc.postRegistroTramiteContrato( pContrato, this.estadoCodigo )
      .subscribe( console.log );

    //this.openDialog( 'La información ha sido guardada exitosamente.', '' );
    //this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
  };

}
