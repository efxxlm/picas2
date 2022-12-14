import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SesionComiteSolicitud, ComiteTecnico, SesionSolicitudVoto, SesionSolicitudObservacionProyecto } from 'src/app/_interfaces/technicalCommitteSession';
import { Router } from '@angular/router';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';

@Component({
    selector: 'app-votacion-solicitud-multiple',
    templateUrl: './votacion-solicitud-multiple.component.html',
    styleUrls: ['./votacion-solicitud-multiple.component.scss']
})
export class VotacionSolicitudMultipleComponent implements OnInit {
    miembros: any[] = ['Juan Lizcano Garcia', 'Fernando José Aldemar Rojas', 'Gonzalo Díaz Mesa'];

    addressForm = this.fb.group({
        aprobaciones: this.fb.array([]),
        proyectos: this.fb.array([]),
    });
    estaEditando = false;

    get aprobaciones() {
        return this.addressForm.get('aprobaciones') as FormArray;
    }

    get proyectos() {
        return this.addressForm.get('proyectos') as FormArray;
    }

    observaciones(i: number) {
        return this.proyectos.controls[i].get('observaciones') as FormArray;
    }

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

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n, e.editor.getLength());
        }
    }

    crearAprobaciones() {
        return this.fb.group({
            nombreParticipante: [],
            sesionSolicitudVotoId: [],
            sesionParticipanteId: [],
            sesionComiteSolicitudId: [],
            aprobacion: [null, Validators.required]

        });
    }

    crearProyectos() {
        return this.fb.group({
            llaveMen: [],
            nombreInstitucion: [],
            nombreSede: [],
            observaciones: this.fb.array([]),

        });
    }

    crearObservaciones() {
        return this.fb.group({
            nombreParticipante: [],
            sesionSolicitudObservacionProyectoId: [],
            sesionComiteSolicitudId: [],
            sesionParticipanteId: [],
            contratacionProyectoId: [],
            observacion: [null, Validators.required]

        });
    }

    constructor(
        private fb: FormBuilder,
        public dialogRef: MatDialogRef<VotacionSolicitudMultipleComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            sesionComiteSolicitud: SesionComiteSolicitud,
            objetoComiteTecnico: ComiteTecnico,
            esVerDetalle: boolean
        },
        private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
        private projectContractingService: ProjectContractingService,
        public dialog: MatDialog,
        private router: Router,
        private projectService: ProjectService,

    ) {

    }

    ngOnInit(): void {

        this.aprobaciones.clear();

        console.log(this.data.sesionComiteSolicitud.sesionSolicitudVoto)
        this.data.sesionComiteSolicitud.sesionSolicitudVoto
            .filter(sv => sv.sesionSolicitudVotoId == 0 || sv.comiteTecnicoFiduciarioId == this.data.sesionComiteSolicitud.comiteTecnicoFiduciarioId).forEach(v => {
                let grupoVotacion = this.crearAprobaciones();

                grupoVotacion.get('nombreParticipante').setValue(v.nombreParticipante);
                grupoVotacion.get('aprobacion').setValue(v.esAprobado);
                if (v.noAplica) grupoVotacion.get('aprobacion').setValue('noAplica');

                grupoVotacion.get('sesionSolicitudVotoId').setValue(v.sesionSolicitudVotoId);
                grupoVotacion.get('sesionParticipanteId').setValue(v.sesionParticipanteId);
                grupoVotacion.get('sesionComiteSolicitudId').setValue(v.sesionComiteSolicitudId);

                this.aprobaciones.push(grupoVotacion)
            })

        this.proyectos.clear();

        this.projectContractingService.getContratacionByContratacionId(this.data.sesionComiteSolicitud.contratacion.contratacionId)
            .subscribe(respuesta => {

                this.data.sesionComiteSolicitud.contratacion = respuesta;
                this.data.sesionComiteSolicitud.contratacion.contratacionProyecto.forEach(cp => {

                    let grupoProyecto = this.crearProyectos();

                    let listaObservaciones = grupoProyecto.get('observaciones') as FormArray;

                    grupoProyecto.get('llaveMen').setValue(cp.proyecto.llaveMen);
                    grupoProyecto.get('nombreInstitucion').setValue(cp.proyecto.institucionEducativa.nombre);
                    grupoProyecto.get('nombreSede').setValue(cp.proyecto.sede.nombre);

                    this.data.sesionComiteSolicitud.sesionSolicitudObservacionProyecto
                        .filter(o => o.contratacionProyectoId == cp.contratacionProyectoId)
                        .forEach(op => {

                            let grupoObservacion = this.crearObservaciones();

                            grupoObservacion.get('nombreParticipante').setValue(op.nombreParticipante);
                            grupoObservacion.get('sesionSolicitudObservacionProyectoId').setValue(op.sesionSolicitudObservacionProyectoId);
                            grupoObservacion.get('sesionComiteSolicitudId').setValue(op.sesionComiteSolicitudId);
                            grupoObservacion.get('sesionParticipanteId').setValue(op.sesionParticipanteId);
                            grupoObservacion.get('contratacionProyectoId').setValue(op.contratacionProyectoId);
                            grupoObservacion.get('observacion').setValue(op.observacion);

                            listaObservaciones.push(grupoObservacion);
                        })


                    this.proyectos.push(grupoProyecto)
                })
            })
    }

    openDialog(modalTitle: string, modalText: string) {
        this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        let sesionComiteSolicitud: SesionComiteSolicitud = {
            sesionComiteSolicitudId: this.data.sesionComiteSolicitud.sesionComiteSolicitudId,
            comiteTecnicoId: this.data.sesionComiteSolicitud.comiteTecnicoId,
            sesionSolicitudVoto: [],
            sesionSolicitudObservacionProyecto: []
        }

        this.aprobaciones.controls.forEach(control => {
            let noAplica: boolean;
            let esAprobado: boolean;
            if (control.get('aprobacion').value === 'noAplica') {
                noAplica = true;
                esAprobado = true;
            } else {
                noAplica = false;
                esAprobado = control.get('aprobacion').value;
            }

            let sesionSolicitudVoto: SesionSolicitudVoto = {
                sesionSolicitudVotoId: control.get('sesionSolicitudVotoId').value,
                sesionComiteSolicitudId: control.get('sesionComiteSolicitudId').value,
                sesionParticipanteId: control.get('sesionParticipanteId').value,
                esAprobado: noAplica ? false : control.get('aprobacion').value,
                noAplica: noAplica,
                comiteTecnicoFiduciarioId: this.data.sesionComiteSolicitud.comiteTecnicoFiduciarioId,
                //observacion: control.get('observaciones').value,

            }

            sesionComiteSolicitud.sesionSolicitudVoto.push(sesionSolicitudVoto);
        })

        this.proyectos.controls.forEach(controlProyecto => {
            let listaObservaciones = controlProyecto.get('observaciones') as FormArray;

            listaObservaciones.controls.forEach(control => {
                let sesionSolicitudObservacionProyecto: SesionSolicitudObservacionProyecto = {
                    sesionSolicitudObservacionProyectoId: control.get('sesionSolicitudObservacionProyectoId').value,
                    sesionComiteSolicitudId: control.get('sesionComiteSolicitudId').value,
                    sesionParticipanteId: control.get('sesionParticipanteId').value,
                    contratacionProyectoId: control.get('contratacionProyectoId').value,
                    observacion: control.get('observacion').value,
                }

                sesionComiteSolicitud.sesionSolicitudObservacionProyecto.push(sesionSolicitudObservacionProyecto);
            })

        })

        sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.AprobadaPorComiteFiduciario;
        sesionComiteSolicitud.sesionSolicitudVoto.forEach(sv => {
            if (sv.esAprobado != true)
                sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.RechazadaPorComiteFiduciario;
        })

        this.fiduciaryCommitteeSessionService.createEditSesionSolicitudVoto(sesionComiteSolicitud)
            .subscribe(respuesta => {
                this.openDialog('', `<b>${respuesta.message}</b>`)
                if (respuesta.code == "200") {
                    this.dialogRef.close(this.data.objetoComiteTecnico);
                    //this.router.navigate(['/comiteTecnico/registrarSesionDeComiteTecnico',this.data.objetoComiteTecnico.comiteTecnicoId,'registrarParticipantes'])


                }

            })

    }

}
