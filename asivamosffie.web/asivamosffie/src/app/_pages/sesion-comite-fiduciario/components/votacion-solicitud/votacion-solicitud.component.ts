import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { ComiteTecnico, SesionComiteSolicitud, SesionSolicitudVoto } from 'src/app/_interfaces/technicalCommitteSession';


@Component({
    selector: 'app-votacion-solicitud',
    templateUrl: './votacion-solicitud.component.html',
    styleUrls: ['./votacion-solicitud.component.scss']
})
export class VotacionSolicitudComponent implements OnInit {

    miembros: any[] = ['Juan Lizcano Garcia', 'Fernando José Aldemar Rojas', 'Gonzalo Díaz Mesa'];

    addressForm = this.fb.array([]);
    estaEditando = false;

    get listaVotacion() {
        return this.addressForm as FormArray;
    }

    get aprobacion() {
        return this.addressForm.get('aprobacion') as FormArray;
    }

    get observaciones() {
        return this.addressForm.get('observaciones') as FormArray;
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

    crearParticipante() {
        return this.fb.group({
            nombreParticipante: [],
            sesionSolicitudVotoId: [],
            sesionParticipanteId: [],
            sesionComiteSolicitudId: [],
            aprobacion: [null, Validators.required],
            observaciones: []
        });
    }

    constructor(
        private fb: FormBuilder,
        public dialogRef: MatDialogRef<VotacionSolicitudComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            sesionComiteSolicitud: SesionComiteSolicitud,
            objetoComiteTecnico: ComiteTecnico,
            esVerDetalle: boolean
        },
        private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
        public dialog: MatDialog,
        private router: Router,

    ) {

    }

    ngOnInit(): void {

        this.data.sesionComiteSolicitud.sesionSolicitudVoto.forEach(v => {
            let grupoVotacion = this.crearParticipante();

            grupoVotacion.get('nombreParticipante').setValue(v.nombreParticipante);
            grupoVotacion.get('aprobacion').setValue(v.esAprobado);
            if (v.noAplica) grupoVotacion.get('aprobacion').setValue('noAplica');
            grupoVotacion.get('observaciones').setValue(v.observacion);

            grupoVotacion.get('sesionSolicitudVotoId').setValue(v.sesionSolicitudVotoId);
            grupoVotacion.get('sesionParticipanteId').setValue(v.sesionParticipanteId);
            grupoVotacion.get('sesionComiteSolicitudId').setValue(v.sesionComiteSolicitudId);

            this.listaVotacion.push(grupoVotacion)
        })

        console.log(this.addressForm.value)

    }

    agregarAprovacion() {
        this.aprobacion.push(this.fb.control(null, Validators.required));
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
        console.log(this.data.sesionComiteSolicitud);

        let sesionComiteSolicitud: SesionComiteSolicitud = {
            sesionComiteSolicitudId: this.data.sesionComiteSolicitud.sesionComiteSolicitudId,
            comiteTecnicoId: this.data.sesionComiteSolicitud.comiteTecnicoId,
            sesionSolicitudVoto: [],
            cantCompromisosFiduciario: null,
            requiereVotacionFiduciario: null,
            desarrolloSolicitudFiduciario: null,
            comiteTecnicoFiduciarioId: null, generaCompromisoFiduciario: null, observacionesFiduciario: null, rutaSoporteVotacionFiduciario: null

        }

        this.listaVotacion.controls.forEach(control => {
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
                comiteTecnicoFiduciarioId: this.data.sesionComiteSolicitud.comiteTecnicoFiduciarioId,
                esAprobado: noAplica ? false : control.get('aprobacion').value,
                noAplica: noAplica,
                observacion: control.get('observaciones').value,

            }

            sesionComiteSolicitud.sesionSolicitudVoto.push(sesionSolicitudVoto);
        })

        sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.AprobadaPorComiteFiduciario;
        sesionComiteSolicitud.sesionSolicitudVoto.forEach(sv => {
            if (sv.esAprobado != true)
                sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.RechazadaPorComiteFiduciario;
        })

        console.log(sesionComiteSolicitud);

        this.fiduciaryCommitteeSessionService.createEditSesionSolicitudVoto(sesionComiteSolicitud)
            .subscribe(respuesta => {
                this.openDialog('', `<b>${respuesta.message}</b>`)
                if (respuesta.code == "200") {
                    this.dialogRef.close(this.data.objetoComiteTecnico);
                }

            })

    }

}
