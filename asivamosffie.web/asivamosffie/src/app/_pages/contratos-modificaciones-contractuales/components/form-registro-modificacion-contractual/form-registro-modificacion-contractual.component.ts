import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-form-registro-modificacion-contractual',
  templateUrl: './form-registro-modificacion-contractual.component.html',
  styleUrls: ['./form-registro-modificacion-contractual.component.scss']
})
export class FormRegistroModificacionContractualComponent implements OnInit {

  @Input() dataFormulario: FormGroup;
  
  archivo                : string;
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
  estaEditando = false;

  constructor ( private dialog: MatDialog,
                private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService,
              ) { }

  ngOnInit(): void {
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
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.routes.navigate(['/contratosModificacionesContractuales']);
    });
  };

  guardar () {
    this.estaEditando = true;
    // console.log( this.dataFormulario );
    let novedad: NovedadContractual = {
      novedadContractualId: this.dataFormulario.get('novedadContractualId').value,
      numeroOtroSi: this.dataFormulario.get('numeroOtroSi').value,
      fechaEnvioFirmaContratista: this.dataFormulario.get('fechaEnvioFirmaContratista').value,
      fechaFirmaContratista: this.dataFormulario.get('fechaFirmaContratista').value,
      fechaEnvioFirmaFiduciaria: this.dataFormulario.get('fechaEnvioFirmaFiduciaria').value,
      fechaFirmaFiduciaria: this.dataFormulario.get('fechaFirmaFiduciaria').value,
      observacionesTramite: this.dataFormulario.get('observacionesTramite').value,
      urlDocumentoSuscrita: this.dataFormulario.get('urlDocumentoSuscrita').value
    };
    this.contratosContractualesSvc.registrarTramiteNovedadContractual(novedad)
    .subscribe((response: Respuesta) => {
        this.openDialog('', response.message);
      },
      err => this.openDialog('', err.message)
    );
  };

}
