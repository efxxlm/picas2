import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-alertas-relevantes',
  templateUrl: './alertas-relevantes.component.html',
  styleUrls: ['./alertas-relevantes.component.scss']
})
export class AlertasRelevantesComponent implements OnInit, OnDestroy {

    @Input() esRegistroNuevo: boolean;
    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionAlertas: any;
    @Output() tieneObservacion = new EventEmitter();
    obsApoyo: any;
    obsSupervisor: any;
    seRealizoPeticion = false;
    formAlertasRelevantes: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraAlertaId = 0;
    gestionAlertas: any;
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
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
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    {
        this.crearFormulario();
    }

    ngOnDestroy(): void {
        if ( this.formAlertasRelevantes.dirty === true && this.seRealizoPeticion === false ) {
            this.guardadoParcialAvanceSemanalSvc.getDataAlertasRelevantes( this.guardadoParcial(), this.seRealizoPeticion )
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataAlertasRelevantes( undefined )
        }
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;

            if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                    && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAlerta.length > 0 )
            {
                this.gestionAlertas = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAlerta[0];
                if ( this.gestionAlertas !== undefined ) {
                    this.seguimientoSemanalGestionObraAlertaId = this.gestionAlertas.seguimientoSemanalGestionObraAlertaId;
                    if ( this.esVerDetalle === false ) {
                        this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalGestionObraAlertaId, this.tipoObservacionAlertas )
                            .subscribe(
                                response => {
                                    this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false && obs.tieneObservacion === true );
                                    this.obsSupervisor  = response.find( obs => obs.archivada === false && obs.esSupervisor === true && obs.tieneObservacion === true );
                                    this.dataHistorial = response.filter( obs => obs.tieneObservacion === true );

                                    if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                        this.tieneObservacion.emit();
                                    }

                                    this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                }
                            );
                    }
                    this.formAlertasRelevantes.setValue(
                        {
                            seIdentificaronAlertas: this.gestionAlertas.seIdentificaronAlertas !== undefined ?
                                                    this.gestionAlertas.seIdentificaronAlertas : null,
                            alerta: this.gestionAlertas.alerta !== undefined ? this.gestionAlertas.alerta : null
                        }
                    );
                }
            }
        }

        if (!this.esRegistroNuevo) this.formAlertasRelevantes.markAllAsTouched();
    }

    crearFormulario() {
        this.formAlertasRelevantes = this.fb.group({
            seIdentificaronAlertas: [ null ],
            alerta: [ null ]
        });
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        this.seRealizoPeticion = true;
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const seguimientoSemanalGestionObra = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seguimientoSemanalGestionObraAlerta: [
                    {
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        seguimientoSemanalGestionObraAlertaId: this.seguimientoSemanalGestionObraAlertaId,
                        seIdentificaronAlertas: this.formAlertasRelevantes.get( 'seIdentificaronAlertas' ).value !== null ?
                                                this.formAlertasRelevantes.get( 'seIdentificaronAlertas' ).value : null,
                        alerta: this.formAlertasRelevantes.get( 'alerta' ).value !== null ?
                                this.formAlertasRelevantes.get( 'alerta' ).value : null
                    }
                ]
            }
        ];

        if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
            if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraAlerta = [
                    {
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        seguimientoSemanalGestionObraAlertaId: this.seguimientoSemanalGestionObraAlertaId,
                        seIdentificaronAlertas: this.formAlertasRelevantes.get( 'seIdentificaronAlertas' ).value !== null ?
                                                this.formAlertasRelevantes.get( 'seIdentificaronAlertas' ).value : null,
                        alerta: this.formAlertasRelevantes.get( 'alerta' ).value !== null ?
                                this.formAlertasRelevantes.get( 'alerta' ).value : null
                    }
                ]
            } else {
                pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
            }
        } else {
            pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
        }

        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                async response => {
                    if ( this.obsApoyo !== undefined ) {
                        this.obsApoyo.archivada = !this.obsApoyo.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyo ).toPromise();
                    }
                    if ( this.obsSupervisor !== undefined ) {
                        this.obsSupervisor.archivada = !this.obsSupervisor.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisor ).toPromise();
                    }

                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/registrarAvanceSemanal/registroSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardadoParcial() {
        return [
            {
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seguimientoSemanalGestionObraAlertaId: this.seguimientoSemanalGestionObraAlertaId,
                seIdentificaronAlertas: this.formAlertasRelevantes.get( 'seIdentificaronAlertas' ).value !== null ?
                                        this.formAlertasRelevantes.get( 'seIdentificaronAlertas' ).value : null,
                alerta: this.formAlertasRelevantes.get( 'alerta' ).value !== null ?
                        this.formAlertasRelevantes.get( 'alerta' ).value : null
            }
        ]
    }

}
