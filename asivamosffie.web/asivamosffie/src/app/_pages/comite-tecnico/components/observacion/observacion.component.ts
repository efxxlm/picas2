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
  listaObservaciones: ContratacionObservacion[] = []
  contratacionObservacion: ContratacionObservacion[] = []

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

      this.sesionComiteSolicitudId = this.data.idsesionComiteSolicitud;
      this.comiteTecnicoId = this.data.idcomiteTecnico;
      this.contratacionProyectoId = this.data.contratacionProyectoid;
      this.contratacionId = this.data.contratacionid;
      this.contratacionObservacion = this.data.contratacionObservacion;

      this.technicalCommitteSessionService
        .getSesionSolicitudObservacionProyecto(this.data.idsesionComiteSolicitud, this.data.contratacionProyectoid)
        .subscribe(observaciones => {
          this.listaObservaciones = observaciones;
          
          this.contratacionObservacion = this.contratacionObservacion.filter( o => o.comiteTecnicoId == this.comiteTecnicoId );

          if (this.contratacionObservacion && this.contratacionObservacion.length > 0) {

            this.observacion = this.contratacionObservacion.length > 0 ? this.contratacionObservacion[0].observacion : null;

            this.contratacionObservacionId = this.contratacionObservacion.length > 0 ? this.contratacionObservacion[0].contratacionObservacionId : 0;
          }
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
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>');

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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  enviarObservacion() {

    let contraracionObservacion: ContratacionObservacion = {
      contratacionObservacionId: this.contratacionObservacionId,
      contratacionProyectoid: this.contratacionProyectoId,
      comiteTecnicoId: this.comiteTecnicoId,
      contratacionId: this.contratacionId,

      observacion: this.observacion,

      contratacionProyecto:{
        proyecto:{
          proyectoId: this.data.proyectoId,
          estadoProyectoCodigo: this.data.estadoProyectoCodigo,
          
        }
      }

    }

    this.technicalCommitteSessionService.crearObservacionProyecto(contraracionObservacion)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`)
        if (respuesta.code == "200")
          this.dialogRef.close(respuesta.data);
      })

  }

}
