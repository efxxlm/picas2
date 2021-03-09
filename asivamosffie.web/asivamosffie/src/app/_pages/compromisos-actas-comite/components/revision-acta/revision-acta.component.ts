import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';
import { TechnicalCommitteSessionService } from '../../../../core/_services/technicalCommitteSession/technical-committe-session.service';
import { CommonService } from '../../../../core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';

@Component({
  selector: 'app-revision-acta',
  templateUrl: './revision-acta.component.html',
  styleUrls: ['./revision-acta.component.scss']
})
export class RevisionActaComponent implements OnInit, OnDestroy {

  acta: any;
  form: FormGroup;
  comentarActa: boolean = false;
  comentarios: boolean = false;
  tablaDecisiones: any[] = [];
  editorStyle = {
    height: '45px'
  };
  observacionInvalida: boolean = false;
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  fechaComentario: Date = new Date();
  miembrosParticipantes: any[] = [];
  temas: any[] = [];
  proposicionesVarios: any[] = [];
  seRealizoPeticion: boolean = false;
  estaEditando = false;
  estadoActa = {
    revisarActa: '2',
    aprobado: '3',
    devuelto: '4'
  };
  tipoSolicitudCodigo = {
    procesoSeleccion: '1',
    contratacion: '2',
    modificacionContractual: '3',
    controversiaContractual: '4',
    defensaJudicial: '5'
  }

  constructor(private routes: Router,
    public dialog: MatDialog,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private commonSvc: CommonService,
    private technicalCommitteeSessionSvc: TechnicalCommitteSessionService,
    private compromisoSvc: CompromisosActasComiteService,
    private projectContractingSvc: ProjectContractingService,
    private comiteTecnicoSvc: TechnicalCommitteSessionService) {
    this.getActa(this.activatedRoute.snapshot.params.id);
    this.crearFormulario();
  }

  ngOnDestroy(): void {
    if (this.form.get('comentarioActa').value !== null && this.seRealizoPeticion === false) {
      this.openDialogConfirmar('', '¿Desea guardar la información registrada?')
    }
  }

  ngOnInit(): void {
  };

  getActa(comiteTecnicoId: number) {
    this.compromisoSvc.getActa(comiteTecnicoId)
      .subscribe((resp: any) => {
        this.acta = resp[0];
        console.log(this.acta);

        for ( let tema of this.acta.sesionComiteTema ) {
          let totalAprobado = 0;
          let totalNoAprobado = 0;
          if ( tema.esProposicionesVarios === undefined || tema.esProposicionesVarios === false ) {
            tema.sesionTemaVoto.forEach( sv => {
              if ( sv.esAprobado === true ) {
                totalAprobado++;
              }
              if ( sv.esAprobado === false ) {
                totalNoAprobado++;
              }
            });
            if ( totalNoAprobado > 0 ) {
              tema.resultadoVotacion = 'No Aprobó';
            } else {
              tema.resultadoVotacion = 'Aprobó';
            }
            tema.totalAprobado = totalAprobado;
            tema.totalNoAprobado = totalNoAprobado;
            this.temas.push( tema );
          }
          if ( tema.esProposicionesVarios === true ) {
            tema.sesionTemaVoto.forEach( sv => {
              if ( sv.esAprobado === true ) {
                totalAprobado++;
              }
              if ( sv.esAprobado === false ) {
                totalNoAprobado++;
              }
            });
            if ( totalNoAprobado > 0 ) {
              tema.resultadoVotacion = 'No Aprobó';
            } else {
              tema.resultadoVotacion = 'Aprobó';
            }
            tema.totalAprobado = totalAprobado;
            tema.totalNoAprobado = totalNoAprobado;
            this.proposicionesVarios.push( tema );
          }
        };

        for (let participante of this.acta.sesionParticipanteView) {
          this.miembrosParticipantes.push(`${participante.nombres} ${participante.apellidos}`)
        };

        this.technicalCommitteeSessionSvc.getComiteTecnicoByComiteTecnicoId(this.activatedRoute.snapshot.params.id)
          .subscribe((response: any) => {
            this.acta.sesionComiteSolicitudComiteTecnico = response.sesionComiteSolicitudComiteTecnico;
          });

      });
  };

  //Formulario comentario de actas
  crearFormulario() {
    this.form = this.fb.group({
      comentarioActa: [null, Validators.required]
    });
  };

  //Limite maximo Quill Editor
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  textoLimpio(texto: string, maxLength: number) {
    if (texto !== undefined) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      console.log(textolimpio.length);
      return textolimpio.length > maxLength ? maxLength : textolimpio.length;
    } else {
      return 0;
    };
  };

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };
  //Modal
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };
  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe(response => {
        if (response === true) {
          this.onSubmit();
        }
      });
  };
  //Submit de la data
  onSubmit() {
    this.estaEditando = true;
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      this.observacionInvalida = true;
      this.openDialog('', '<b>Falta registrar información.</b>');
      return;
    };

    const value = this.form.get('comentarioActa').value;
    const observaciones = {
      comiteTecnicoId: this.acta.comiteTecnicoId,
      observaciones: value
    };

    this.compromisoSvc.postComentariosActa(observaciones)
      .subscribe((resp: any) => {
        this.seRealizoPeticion = true;
        this.openDialog('', `<b>${resp.message}</b>`);
        this.routes.navigate(['/compromisosActasComite']);
      });

  };

  innerObservacion(observacion: string) {
    if (observacion !== undefined) {
      const observacionHtml = observacion.replace('"', '');
      return `<b>${observacionHtml}</b>`;
    };
  };
  //Aprobar acta
  aprobarActa(comiteTecnicoId) {
    //Al aprobar acta redirige al componente principal
    this.compromisoSvc.aprobarActa(comiteTecnicoId)
      .subscribe(
        response => {
          this.seRealizoPeticion = true;
          this.openDialog('', `<b>${response.message}</b>`);
          this.routes.navigate(['/compromisosActasComite']);
        },
        err => this.openDialog('', `<b>${err.message}</b>`)
      )
  };

  //Descargar acta en formato pdf
  getActaPdf(comiteTecnicoId, numeroComite) {

    if (this.acta.esComiteFiduciario === true) {
      this.comiteTecnicoSvc.getPlantillaActaBySesionComiteSolicitudFiduciarioId(comiteTecnicoId)
        .subscribe((resp: any) => {

          const documento = `Acta Preliminar ${numeroComite}.pdf`;
          const text = documento,
            blob = new Blob([resp], { type: 'application/pdf' }),
            anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
          anchor.click();

        })

    } else {

      this.comiteTecnicoSvc.getPlantillaActaBySesionComiteSolicitudId(comiteTecnicoId)
        .subscribe((resp: any) => {

          const documento = `Acta Preliminar ${numeroComite}.pdf`;
          const text = documento,
            blob = new Blob([resp], { type: 'application/pdf' }),
            anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
          anchor.click();

        })


    }
  }

};