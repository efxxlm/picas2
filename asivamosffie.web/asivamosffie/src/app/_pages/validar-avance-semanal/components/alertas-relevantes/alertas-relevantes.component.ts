import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-alertas-relevantes',
  templateUrl: './alertas-relevantes.component.html',
  styleUrls: ['./alertas-relevantes.component.scss']
})
export class AlertasRelevantesComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionAlertas: any;
    formAlertasRelevantes: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraAlertaId = 0;
    seguimientoSemanalObservacionId = 0;
    contadorObservacionApoyo = 0;
    gestionAlertas: any;
    observacionApoyo: any[] = [];
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
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

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    { }

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
                    if ( this.gestionAlertas.observacionApoyoId !== undefined || this.gestionAlertas.observacionSupervisorId !== undefined ) {
                        this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalGestionObraAlertaId, this.tipoObservacionAlertas )
                            .subscribe(
                                response => {
                                    this.observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                    const observacionSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                    this.dataHistorial = response.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
                                    this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                    if ( observacionSupervisor.length > 0 ) {
                                        if ( observacionSupervisor[0].observacion !== undefined ) {
                                            if ( observacionSupervisor[0].observacion.length > 0 ) {
                                                this.formAlertasRelevantes.get( 'observaciones' ).setValue( observacionSupervisor[0].observacion );
                                            }
                                        }
                                        this.seguimientoSemanalObservacionId = observacionSupervisor[0].seguimientoSemanalObservacionId;
                                        this.formAlertasRelevantes.get( 'tieneObservaciones' ).setValue( this.gestionAlertas.tieneObservacionSupervisor );
                                        this.formAlertasRelevantes.get( 'fechaCreacion' ).setValue( observacionSupervisor[0].fechaCreacion );
                                    }
                                }
                            );
                    }
                }
            }
        }
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
        if ( this.gestionAlertas.tieneObservacionApoyo === true && this.formAlertasRelevantes.get( 'tieneObservaciones' ).value === false && this.contadorObservacionApoyo === 0 ) {
            this.contadorObservacionApoyo++;
            this.openDialog( '', '<b>Le recomendamos verificar su respuesta;<br>Tenga en cuenta que el apoyo a la supervisi??n si tuvo observaciones.</b>' );
            return;
        }
        if ( this.formAlertasRelevantes.get( 'tieneObservaciones' ).value === false && this.formAlertasRelevantes.get( 'observaciones' ).value !== null ) {
            this.formAlertasRelevantes.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionAlertas,
            observacionPadreId: this.seguimientoSemanalGestionObraAlertaId,
            observacion: this.formAlertasRelevantes.get( 'observaciones' ).value,
            tieneObservacion: this.formAlertasRelevantes.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }

        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
