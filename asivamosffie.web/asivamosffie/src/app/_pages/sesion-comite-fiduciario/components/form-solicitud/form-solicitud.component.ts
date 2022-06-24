import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { SesionComiteSolicitud, SesionParticipante, SesionSolicitudCompromiso, TiposSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ContratacionProyecto, EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { VotacionSolicitudComponent } from '../votacion-solicitud/votacion-solicitud.component';

@Component({
    selector: 'app-form-solicitud',
    templateUrl: './form-solicitud.component.html',
    styleUrls: ['./form-solicitud.component.scss']
})
export class FormSolicitudComponent implements OnInit, OnChanges {

    @Input() sesionComiteSolicitud: SesionComiteSolicitud;
    @Input() listaMiembros: SesionParticipante[];
    @Input() fechaMaxima: any;
    @Input() fechaComite: Date;
    @Input() EstadosolicitudActa: any;
    @Input() esRegistroNuevo: boolean;

    @Output() validar: EventEmitter<boolean> = new EventEmitter();

    tiposSolicitud = TiposSolicitud;

    fechaSolicitud: Date;
    numeroSolicitud: string;
    justificacion: string;

    tieneVotacion: boolean = true;
    cantidadAprobado: number = 0;
    cantidadNoAprobado: number = 0;
    cantidadNoAplica = 0
    resultadoVotacion: string = '';

    proyectos: ContratacionProyecto[];
    listaMiembrosNoFilter: Usuario[];

    addressForm = this.fb.group({
        desarrolloSolicitud: [],
        estadoSolicitud: [null, Validators.required],
        observaciones: [null, Validators.required],
        url: null,
        tieneCompromisos: [null, Validators.required],
        cuantosCompromisos: [null, Validators.required],
        compromisos: this.fb.array([])
    });

    estadosArray: Dominio[] = [];

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

    get compromisos() {
        return this.addressForm.get('compromisos') as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private commonService: CommonService,
        private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
        private technicalCommitteSessionService: TechnicalCommitteSessionService,
        public dialog: MatDialog,
        private router: Router,

    ) {

    }
    ngOnChanges(changes: SimpleChanges): void {
        if (changes.sesionComiteSolicitud.currentValue) {
            this.cargarRegistro();
        }
    }

    ActualizarProyectos(lista) {
        console.log(lista)
        this.proyectos = lista;
    }

    getMostrarProyectos() {
        if (this.sesionComiteSolicitud.tipoSolicitudCodigo == this.tiposSolicitud.Contratacion)
            return 'block';
        else
            return 'none';
    }
    ngOnInit(): void {
        // console.log( this.fechaMaxima );
        this.addressForm.valueChanges
            .subscribe(value => {
                if (value.cuantosCompromisos > 10) { value.cuantosCompromisos = 10; }
            });
        this.commonService.listaUsuarios().then(respuesta => {
            this.listaMiembrosNoFilter = respuesta;
        });
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n, e.editor.getLength());
        }
    }

    textoLimpio(texto: string) {
        let saltosDeLinea = 0;
        // saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
        // saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

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

    borrarArray(borrarForm: any, i: number) {
        borrarForm.removeAt(i);
    }

    eliminarCompromiso(i: number) {
        this.openDialogSiNo('', '<b>¿Está seguro de eliminar este compromiso?</b>', i);
    }

    openDialogSiNo(modalTitle: string, modalText: string, e: number) {
        let dialogRef = this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText, siNoBoton: true }
        });
        dialogRef.afterClosed().subscribe(result => {
            console.log(`Dialog result: ${result}`);
            if (result === true) {
                this.eliminarCompromisos(e);
            }
        });
    }

    agregaCompromiso() {
        this.compromisos.push(this.crearCompromiso());
    }

    crearCompromiso() {
        return this.fb.group({
            sesionSolicitudCompromisoId: [],
            sesionComiteSolicitudId: [],
            tarea: [null, Validators.compose([
                Validators.required, Validators.minLength(1), Validators.maxLength(500)])
            ],
            responsable: [null, Validators.required],
            fecha: [null, Validators.required]
        });
    }

    changeCompromisos(requiereCompromisos) {

        if (requiereCompromisos.value === false) {
            console.log(requiereCompromisos.value);
            this.technicalCommitteSessionService.eliminarCompromisosSolicitud(this.sesionComiteSolicitud.sesionComiteSolicitudId)
                .subscribe(respuesta => {
                    if (respuesta.code == "200") {
                        this.compromisos.clear();
                        this.addressForm.get("cuantosCompromisos").setValue(null);
                    }
                })
        }
    }

    eliminarCompromisos(i) {

        let compromiso = this.compromisos.controls[i];

        this.technicalCommitteSessionService.deleteSesionComiteCompromiso(compromiso.get('sesionSolicitudCompromisoId').value)
            .subscribe(respuesta => {
                if (respuesta.code == "200") {
                    this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>');
                    this.compromisos.removeAt(i)
                    this.addressForm.get("cuantosCompromisos").setValue(this.compromisos.length);
                }
            })
    }


    validarCompromisosDiligenciados(): boolean {
        let vacio = true;
        this.compromisos.controls.forEach(control => {
            if (control.value.tarea ||
                control.value.responsable ||
                control.value.fecha
            )
                vacio = false;
        })

        return vacio;
    }

    CambioCantidadCompromisos() {
        if (this.estaEditando) this.compromisos.markAllAsTouched();
        const FormGrupos = this.addressForm.value;
        if (FormGrupos.cuantosCompromisos > this.compromisos.length && FormGrupos.cuantosCompromisos < 100) {
            while (this.compromisos.length < FormGrupos.cuantosCompromisos) {
                this.compromisos.push(this.crearCompromiso());
            }
        } else if (FormGrupos.cuantosCompromisos <= this.compromisos.length && FormGrupos.cuantosCompromisos >= 0) {
            if (this.validarCompromisosDiligenciados()) {

                while (this.compromisos.length > FormGrupos.cuantosCompromisos) {
                    this.borrarArray(this.compromisos, this.compromisos.length - 1);
                }

            }
            else {

                this.openDialog('', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos</b>');
                this.addressForm.get('cuantosCompromisos').setValue(this.compromisos.length);

            }
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    getObservableEstadoSolicitud() {
        return this.addressForm.get('estadoSolicitud').valueChanges;
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        let tipoSolicitudCodigo: string;

        if (this.proyectos)
            this.proyectos.forEach(p => {
                let proyecto = p.proyecto
                tipoSolicitudCodigo = p.contratacion.tipoSolicitudCodigo;

                p.proyecto = {
                    proyectoId: proyecto.proyectoId,
                    estadoProyectoObraCodigo: proyecto.estadoProyectoObraCodigo,
                    estadoProyectoInterventoriaCodigo: proyecto.estadoProyectoInterventoriaCodigo,

                }
                //p.proyecto.estadoProyecto = p.proyecto.estadoProyecto ? p.proyecto.estadoProyecto.codigo : null
            })

        console.log(tipoSolicitudCodigo);

        let Solicitud: SesionComiteSolicitud = {
            sesionComiteSolicitudId: this.sesionComiteSolicitud.sesionComiteSolicitudId,
            comiteTecnicoId: this.sesionComiteSolicitud.comiteTecnicoId,
            comiteTecnicoFiduciarioId: this.sesionComiteSolicitud.comiteTecnicoFiduciarioId,
            estadoCodigo: this.addressForm.get('estadoSolicitud').value,

            observacionesFiduciario: this.addressForm.get('observaciones').value,
            rutaSoporteVotacionFiduciario: this.addressForm.get('url').value,
            generaCompromisoFiduciario: this.addressForm.get('tieneCompromisos').value,
            cantCompromisosFiduciario: this.addressForm.get('cuantosCompromisos').value,
            desarrolloSolicitudFiduciario: this.addressForm.get('desarrolloSolicitud').value,
            tipoSolicitud: this.sesionComiteSolicitud.tipoSolicitudCodigo,
            sesionSolicitudCompromiso: [],
            contratacion: {
                contratacionProyecto: this.proyectos ? this.proyectos : null,
                tipoSolicitudCodigo: tipoSolicitudCodigo,
            }

        }

        this.compromisos.controls.forEach(control => {
            let sesionSolicitudCompromiso: SesionSolicitudCompromiso = {
                tarea: control.get('tarea').value,
                responsableSesionParticipanteId: control.get('responsable').value ? control.get('responsable').value.sesionParticipanteId : null,
                fechaCumplimiento: control.get('fecha').value,
                sesionSolicitudCompromisoId: control.get('sesionSolicitudCompromisoId').value,
                sesionComiteSolicitudId: this.sesionComiteSolicitud.sesionComiteSolicitudId,

            }

            Solicitud.sesionSolicitudCompromiso.push(sesionSolicitudCompromiso);
        })

        console.log(Solicitud)

        this.fiduciaryCommitteeSessionService.createEditActasSesionSolicitudCompromiso(Solicitud)
            .subscribe(respuesta => {
                this.openDialog('', `<b>${respuesta.message}</b>`)
                console.log(respuesta.data)
                this.validar.emit(respuesta.data);
                if (respuesta.code == "200" && !respuesta.data)
                    this.router.navigate(['/comiteFiduciario/crearActa', this.sesionComiteSolicitud.comiteTecnicoFiduciarioId])
            })

    }

    cargarRegistro() {

        console.log(this.sesionComiteSolicitud)

        let estados: string[] = ['2', '4', '6']

        this.commonService.listaEstadoSolicitud()
            .subscribe(response => {

                this.estadosArray = response.filter(s => estados.includes(s.codigo));
                if (this.sesionComiteSolicitud.requiereVotacionFiduciario) {
                    this.sesionComiteSolicitud.sesionSolicitudVoto.filter(sv => sv.comiteTecnicoFiduciarioId == this.sesionComiteSolicitud.comiteTecnicoFiduciarioId).forEach(sv => {
                        if (sv.noAplica) this.cantidadNoAplica = this.cantidadNoAplica + 1
                        if (sv.esAprobado)
                            this.cantidadAprobado++;
                        else
                            this.cantidadNoAprobado++;
                    })

                    if (this.sesionComiteSolicitud.sesionSolicitudVoto.filter(sv => sv.comiteTecnicoFiduciarioId == this.sesionComiteSolicitud.comiteTecnicoFiduciarioId).length === this.cantidadNoAplica) {
                        this.resultadoVotacion = 'No aplica'
                        this.cantidadAprobado = 0
                        this.cantidadNoAprobado = 0
                    } else {
                        if (this.cantidadNoAprobado == 0) {
                            this.resultadoVotacion = 'Aprobó'
                            this.estadosArray = this.estadosArray.filter(e => e.codigo == EstadosSolicitud.AprobadaPorComiteFiduciario)
                        } else if (this.cantidadAprobado == 0) {
                            this.resultadoVotacion = 'No Aprobó'
                            this.estadosArray = this.estadosArray.filter(e => [EstadosSolicitud.RechazadaPorComiteFiduciario, EstadosSolicitud.DevueltaPorComiteFiduciario].includes(e.codigo))
                        } else if (this.cantidadAprobado > this.cantidadNoAprobado) {
                            this.resultadoVotacion = 'Aprobó'
                        } else if (this.cantidadAprobado <= this.cantidadNoAprobado) {
                            this.resultadoVotacion = 'No Aprobó'
                        }
                    }
                }
            });

        this.addressForm.get('estadoSolicitud').setValue(this.sesionComiteSolicitud.estadoCodigo)
        this.addressForm.get('observaciones').setValue(this.sesionComiteSolicitud.observacionesFiduciario)
        this.addressForm.get('url').setValue(this.sesionComiteSolicitud.rutaSoporteVotacionFiduciario)
        this.addressForm.get('tieneCompromisos').setValue(this.sesionComiteSolicitud.generaCompromisoFiduciario)
        this.addressForm.get('cuantosCompromisos').setValue(this.sesionComiteSolicitud.cantCompromisosFiduciario)
        this.addressForm.get('desarrolloSolicitud').setValue(this.sesionComiteSolicitud.desarrolloSolicitudFiduciario)

        if (!this.esRegistroNuevo) {
            this.estaEditando = true;
            this.addressForm.markAllAsTouched();
        }

        this.commonService.listaUsuarios().then((respuesta) => {

            this.listaMiembros.forEach(m => {
                let usuario: Usuario = respuesta.find(u => u.usuarioId == m.usuarioId);
                m.nombre = `${usuario.primerNombre} ${usuario.segundoNombre} ${usuario.primerApellido} ${usuario.segundoApellido}`
            })

            this.sesionComiteSolicitud.sesionSolicitudCompromiso.forEach(c => {
                let grupoCompromiso = this.crearCompromiso();
                let responsableSeleccionado = this.listaMiembros.find(m => m.sesionParticipanteId == c.responsableSesionParticipanteId)

                grupoCompromiso.get('tarea').setValue(c.tarea);
                grupoCompromiso.get('responsable').setValue(responsableSeleccionado);
                grupoCompromiso.get('fecha').setValue(c.fechaCumplimiento);
                grupoCompromiso.get('sesionSolicitudCompromisoId').setValue(c.sesionSolicitudCompromisoId);
                grupoCompromiso.get('sesionComiteSolicitudId').setValue(this.sesionComiteSolicitud.sesionComiteSolicitudId);

                this.compromisos.push(grupoCompromiso)
            })

        });

        this.tieneVotacion = this.sesionComiteSolicitud.requiereVotacionFiduciario;

        if (this.sesionComiteSolicitud.tipoSolicitudCodigo == TiposSolicitud.AperturaDeProcesoDeSeleccion) {
            this.justificacion = this.sesionComiteSolicitud.procesoSeleccion.justificacion
        }

        if (!this.esRegistroNuevo) {
            this.estaEditando = true;
            this.addressForm.markAllAsTouched();
        }
    }

    Observaciones(elemento: SesionComiteSolicitud) {
        console.log(elemento.tipoSolicitudCodigo, elemento);
        if (elemento?.sesionSolicitudVoto) {
            elemento.sesionSolicitudVoto.forEach(voto => {
                let usuario: Usuario = this.listaMiembrosNoFilter.find(m => m.usuarioId == voto?.sesionParticipante?.usuarioId);
                voto.nombreParticipante = `${usuario.primerNombre} ${usuario.primerApellido}`;
            });
        }

        if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.AperturaDeProcesoDeSeleccion) {
            const dialog = this.dialog.open(VotacionSolicitudComponent, {
                width: '70em',
                data: { sesionComiteSolicitud: elemento, esVerDetalle: true }
            });

            dialog.afterClosed().subscribe(c => {
                //no se que pasa
            });
        }
    }


}
