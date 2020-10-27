import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ContratacionObservacion } from 'src/app/_interfaces/project-contracting';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { SesionSolicitudObservacionProyecto } from 'src/app/_interfaces/technicalCommitteSession';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-observacion',
  templateUrl: './observacion.component.html',
  styleUrls: ['./observacion.component.scss']
})
export class ObservacionComponent implements OnInit {

  observacion: string;
  contratacionObservacionId: number;
  comiteTecnicoId: number;
  sesionComiteSolicitudId: number;
  contratacionProyectoId: number;
  contratacionId: number;
  listaObservaciones: SesionSolicitudObservacionProyecto[] = []

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
    public dialogRef: MatDialogRef<ObservacionComponent>,
    private technicalCommitteSessionService: TechnicalCommitteSessionService,
    public dialog: MatDialog,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    @Inject(MAT_DIALOG_DATA) public data
  ) {
    //this.declararObservacion();
    console.log(data)
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe(parametros => {
      this.sesionComiteSolicitudId = this.data.idsesionComiteSolicitud;
      this.comiteTecnicoId = this.data.idcomiteTecnico;
      this.contratacionProyectoId = this.data.contratacionProyectoid;
      this.contratacionId = this.data.contratacionid;

      this.technicalCommitteSessionService
        .getSesionSolicitudObservacionProyecto(this.data.idsesionComiteSolicitud, this.data.contratacionProyectoid)
        .subscribe(observaciones => {
          this.listaObservaciones = observaciones;
          if (this.listaObservaciones && this.listaObservaciones.length > 0) {
            this.observacion = this.listaObservaciones[0]['contratacionProyecto'].contratacion.contratacionObservacion[0] ?
              this.listaObservaciones[0]['contratacionProyecto'].contratacion.contratacionObservacion[0].observacion : null;

            this.contratacionObservacionId = this.listaObservaciones[0]['contratacionProyecto'].contratacion.contratacionObservacion[0] ?
              this.listaObservaciones[0]['contratacionProyecto'].contratacion.contratacionObservacion[0].contratacionObservacionId : 0;
          }
        })


    })


  }

  // private declararObservacion() {
  //   this.observacion = new FormControl(null, [Validators.required]);
  // }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  enviarObservacion() {

    let contraracionObservacion: ContratacionObservacion = {
      contratacionObservacionId: this.contratacionObservacionId,
      comiteTecnicoId: this.comiteTecnicoId,
      contratacionId: this.contratacionId,

      observacion: this.observacion,

    }

    this.technicalCommitteSessionService.crearObservacionProyecto(contraracionObservacion)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message)
        if (respuesta.code == "200")
          this.dialogRef.close(contraracionObservacion);
      })

  }

}
