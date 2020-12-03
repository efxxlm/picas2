import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ContratoPerfil } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-hojas-vida-verificar-requisitos',
  templateUrl: './hojas-vida-verificar-requisitos.component.html',
  styleUrls: ['./hojas-vida-verificar-requisitos.component.scss']
})
export class HojasVidaVerificarRequisitosComponent implements OnInit {
  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    construccionPerfilObservacionId:[]
  });

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  @Input() observacionesCompleted;
  @Input() perfil: any;

  constructor(
      private dialog: MatDialog, 
      private fb: FormBuilder,
      private faseUnoConstruccionService: FaseUnoConstruccionService,
      
      ) { }

  ngOnInit(): void {
    if (this.perfil){

        this.addressForm.get('tieneObservaciones').setValue(this.perfil.tieneObservacionesApoyo)
        this.addressForm.get('observaciones').setValue(this.perfil.observacionApoyo ? this.perfil.observacionApoyo.observacion : null)
        this.addressForm.get('construccionPerfilObservacionId').setValue(this.perfil.observacionApoyo ? this.perfil.observacionApoyo.construccionPerfilObservacionId : null)
  
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  };

  innerObservacion ( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    };
  };
  
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  onSubmit(){

    let ConstraccionPerfil = {
      construccionPerfilId: this.perfil.construccionPerfilId,
      tieneObservacionesApoyo: this.addressForm.value.tieneObservaciones,

      construccionPerfilObservacion: [
        {
          ConstruccionPerfilObservacionId: this.addressForm.value.construccionPerfilObservacionId,
          construccionPerfilId: this.perfil.construccionPerfilId,
          esSupervision: false,
          esActa: false,
          observacion: this.addressForm.value.observaciones,

        }
      ]
    }

    console.log();

    this.faseUnoConstruccionService.createEditObservacionPerfil( ConstraccionPerfil )
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code == "200"){
          location.reload()
        }
          //this.createEdit.emit(true);
      });

  }

}
