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
  @Input() fechaTramite  : Date;
  estadoCodigo  : string;
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

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
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
      let fechaEnvioFirma = new Date( this.dataFormulario.get( 'fechaEnvioParaFirmaContratista' ).value );
      //fechaEnvioFirma = `${ fechaEnvioFirma.getFullYear() }/${ fechaEnvioFirma.getMonth() + 1 }/${ fechaEnvioFirma.getDate() }`;
      let fechaEnvioFirmas = fechaEnvioFirma.toISOString();
      pContrato.append( 'fechaEnvioFirma', `${ fechaEnvioFirmas }` );
      this.estadoCodigo = this.estadoCodigos.enFirmaContratista;
    }

    if ( this.dataFormulario.get( 'fechaFirmaPorParteContratista' ).value !== null ) {
      let fechaFirmaContratista = new Date( this.dataFormulario.get( 'fechaFirmaPorParteContratista' ).value );
      //fechaFirmaContratista = `${ fechaFirmaContratista.getFullYear() }/${ fechaFirmaContratista.getMonth() + 1 }/${ fechaFirmaContratista.getDate() }`;
      let fechaFirmaContratistas = fechaFirmaContratista.toISOString();
      pContrato.append( 'fechaFirmaContratista', `${ fechaFirmaContratistas }` );
    } else {
      pContrato.append( 'fechaFirmaContratista', null );
    }

    if ( this.dataFormulario.get( 'fechaEnvioParaFirmaFiduciaria' ).value !== null ) {
      let fechaFirmaFiduciaria = new Date( this.dataFormulario.get( 'fechaEnvioParaFirmaFiduciaria' ).value );
      //fechaFirmaFiduciaria = `${ fechaFirmaFiduciaria.getFullYear() }/${ fechaFirmaFiduciaria.getMonth() + 1 }/${ fechaFirmaFiduciaria.getDate() }`; 
      let fechaFirmaFiduciarias = fechaFirmaFiduciaria.toISOString();
      pContrato.append( 'fechaFirmaFiduciaria', `${ fechaFirmaFiduciarias }` );
      this.estadoCodigo = this.estadoCodigos.enFirmaFiduciaria;
    } else {
      pContrato.append( 'fechaFirmaFiduciaria', null );
    }

    if ( this.dataFormulario.get( 'fechaFirmaPorParteFiduciaria' ).value !== null ) {
      let fechaFirmaContrato = new Date( this.dataFormulario.get( 'fechaFirmaPorParteFiduciaria' ).value );
      //fechaFirmaContrato = `${ fechaFirmaContrato.getFullYear() }/${ fechaFirmaContrato.getMonth() + 1 }/${ fechaFirmaContrato.getDate() }`;
      let fechaFirmaContratos = fechaFirmaContrato.toISOString();
      pContrato.append( 'fechaFirmaContrato', `${ fechaFirmaContratos }` );
    } else {
      pContrato.append( 'fechaFirmaContrato', null );
    }

    if ( this.dataFormulario.get( 'observaciones' ).value ) {
      pContrato.append( 'observaciones', `${ this.dataFormulario.get( 'observaciones' ).value }` );
    }

    if ( this.dataFormulario.get( 'rutaDocumento' ).value !== null ) {
      pContrato.append( 'rutaDocumento', this.dataFormulario.get( 'rutaDocumento' ).value );
      this.estadoCodigo = this.estadoCodigos.firmado;
    }

    this.contratosContractualesSvc.postRegistroTramiteContrato( pContrato, this.estadoCodigo )
      .subscribe( 
        ( resp: any ) => {
          this.openDialog( this.textoLimpioMessage( resp.message ), '' );
          this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
        },
        ( error: any ) => {
          console.log( error );
          this.openDialog( error.message, '' );
        }
      );

    //
    //
  };

}
