import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ContratacionObservacion } from 'src/app/_interfaces/project-contracting';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { SesionSolicitudObservacionProyecto } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-observacion',
  templateUrl: './observacion.component.html',
  styleUrls: ['./observacion.component.scss']
})
export class ObservacionComponent implements OnInit {

  observacion: string;
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
                private technicalCommitteSessionService: TechnicalCommitteSessionService,
                public dialog: MatDialog,
                private router: Router,
                private activatedRoute: ActivatedRoute,

             ) 
  {
    //this.declararObservacion();
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      this.sesionComiteSolicitudId = parametros.idsesionComiteSolicitud;
      this.comiteTecnicoId = parametros.idcomiteTecnico;
      this.contratacionProyectoId = parametros.idcontratacionProyecto;
      this.contratacionId = parametros.idcontratacion;

    this.technicalCommitteSessionService.getSesionSolicitudObservacionProyecto( this.sesionComiteSolicitudId, this.contratacionProyectoId )
      .subscribe( observaciones => {
        this.listaObservaciones = observaciones;
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
      comiteTecnicoId: this.comiteTecnicoId,
      contratacionId: this.contratacionId,

      observacion: this.observacion,

    }

    this.technicalCommitteSessionService.crearObservacionProyecto( contraracionObservacion )
      .subscribe( respuesta => {
        this.openDialog( '', respuesta.message )
        if (respuesta.code == "200")
          this.router.navigate(['/comiteTecnico/crearActa', this.comiteTecnicoId]);
      })

  }

}
