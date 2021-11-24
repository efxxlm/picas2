import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-observaciones',
  templateUrl: './dialog-observaciones.component.html',
  styleUrls: ['./dialog-observaciones.component.scss']
})
export class DialogObservacionesComponent implements OnInit {

  observaciones = this.fb.group({
    ajustePragramacionObservacionId: [null, Validators.required],
    ajusteProgramacionId: [null, Validators.required],
    observaciones: [null, Validators.required],
    esObra: [null, Validators.required],
    archivoCargueId: [true, Validators.required],
    tieneObservacionesProgramacionObra: [null, Validators.required],
    tieneObservacionesFlujoInversion: [null, Validators.required],
  });

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private reprogrammingSvc: ReprogrammingService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data,
    ) {
  }

  ngOnInit(): void {
    if(this.data.dataFile.tempAjustePragramacionObservacion!= null){
      this.observaciones.patchValue(this.data.dataFile.tempAjustePragramacionObservacion);
    }
  }


  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if (texto) {
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
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data: { modalTitle, modalText }
    });
  };

  openDialogGuardar(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
     this.router.navigate(['/registrarAjusteProgramacion'], {});
    });
  }

  onClose(): void {
    this.dialog.closeAll();
  }

  guardar() {
    this.observaciones.value.ajusteProgramacionId = this.data.ajusteProgramacionInfo.ajusteProgramacionId;
    this.observaciones.value.esObra = this.data.esObra;

    const ajustePragramacionObservacion = [];
    ajustePragramacionObservacion.push(
      {
        ajustePragramacionObservacionId: this.observaciones.value.ajustePragramacionObservacionId ?? 0,
        ajusteProgramacionId: this.observaciones.value.ajusteProgramacionId,
        observaciones: this.observaciones.value.observaciones,
        esObra: this.data.esObra,
        archivoCargueId: this.data.dataFile.archivoCargueId
      }
    );

    const ajusteProgramacion = {
      ajusteProgramacionId: this.observaciones.value.ajusteProgramacionId,
      tieneObservacionesProgramacionObra: this.data.esObra == true ? true : this.data.ajusteProgramacionInfo?.tieneObservacionesProgramacionObra,
      tieneObservacionesFlujoInversion: this.data.esObra != true ? true : this.data.ajusteProgramacionInfo?.tieneObservacionesFlujoInversion,
      ajustePragramacionObservacion: ajustePragramacionObservacion
    }
    this.createEditObservacionAjusteProgramacion(ajusteProgramacion);
  }

  createEditObservacionAjusteProgramacion(pAjusteProgramacion: any) {
    this.reprogrammingSvc.createEditObservacionAjusteProgramacion(pAjusteProgramacion, this.data.esObra)
      .subscribe((respuesta: Respuesta) => {
        this.onClose();
        this.openDialogGuardar('', respuesta.message);
        return;
      })
  }

}
