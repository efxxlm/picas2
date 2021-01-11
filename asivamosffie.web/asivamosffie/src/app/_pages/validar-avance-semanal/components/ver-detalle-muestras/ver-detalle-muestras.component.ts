import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { FormArray, FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-ver-detalle-muestras',
  templateUrl: './ver-detalle-muestras.component.html',
  styleUrls: ['./ver-detalle-muestras.component.scss']
})
export class VerDetalleMuestrasComponent implements OnInit {

    formMuestra: FormGroup;
    ensayoLaboratorio: any;
    seguimientoSemanalId: number;
    tipoMuestraEnsayo: string;
    esVerDetalle = false;
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

    get muestras() {
        return this.formMuestra.get( 'muestras' ) as FormArray;
    }

    constructor(
        private location: Location,
        private fb: FormBuilder,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private dialog: MatDialog,
        private activatedRoute: ActivatedRoute )
    {
        this.crearFormulario();
        this.getMuestras();
        this.activatedRoute.url
            .subscribe(
                response => {
                    const pathDetalle = response.filter( urlSegment => urlSegment.path === 'verDetalleAvanceSemanal' );
                    if ( pathDetalle.length > 0 ) {
                        this.esVerDetalle = true;
                    }
                }
            )
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
        this.location.back();
    }

    getMuestras() {
        this.avanceSemanalSvc.getEnsayoLaboratorioMuestras( Number( this.activatedRoute.snapshot.params.idEnsayo ) )
        .subscribe(
            ensayos => {
                this.verificarAvanceSemanalSvc.tipoObservaciones()
                    .subscribe(
                        tipoObservaciones => {
                            this.ensayoLaboratorio = ensayos;
                            this.tipoMuestraEnsayo = tipoObservaciones.gestionCalidad.muestrasEnsayo;
                            this.seguimientoSemanalId = this.ensayoLaboratorio.seguimientoSemanalGestionObraCalidad.seguimientoSemanalGestionObra.seguimientoSemanalId;
                            if ( this.ensayoLaboratorio.ensayoLaboratorioMuestra.length > 0 ) {
                                this.muestras.clear();
                                for ( const muestra of this.ensayoLaboratorio.ensayoLaboratorioMuestra ) {
                                    this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, muestra.ensayoLaboratorioMuestraId, this.tipoMuestraEnsayo )
                                        .subscribe(
                                            response => {
                                                const obsApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                                const obsSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                                let observacionApoyo: string;
                                                let observacionSupervisor: string;
                                                let dataHistorial = [];
                                                let seguimientoSemanalObservacionId = 0;
                                                let semaforoMuestra: string;
                                                dataHistorial = response.filter( obs => obs.archivada === true );
                                                if ( obsApoyo.length > 0 ) {
                                                    if ( obsApoyo[0].observacion !== undefined ) {
                                                        if ( obsApoyo[0].observacion.length > 0 ) {
                                                            observacionApoyo = obsApoyo[0].observacion;
                                                        }
                                                    }
                                                }
                                                if ( obsSupervisor.length > 0 ) {
                                                    seguimientoSemanalObservacionId = obsSupervisor[0].seguimientoSemanalObservacionId;
                                                    if ( obsSupervisor[0].observacion !== undefined ) {
                                                        if ( obsSupervisor[0].observacion.length > 0 ) {
                                                            observacionSupervisor = obsSupervisor[0].observacion;
                                                        }
                                                    }
                                                }
                                                if ( muestra.registroCompletoObservacionSupervisor === true ) {
                                                    semaforoMuestra = 'completo';
                                                }
                                                if (    muestra.registroCompletoObservacionSupervisor === false ) {
                                                    semaforoMuestra = 'en-proceso';
                                                }
                                                this.muestras.push(
                                                    this.fb.group(
                                                        {
                                                            semaforoMuestra: semaforoMuestra !== undefined ? semaforoMuestra : 'sin-diligenciar',
                                                            ensayoLaboratorioMuestraId: muestra.ensayoLaboratorioMuestraId,
                                                            gestionObraCalidadEnsayoLaboratorioId: muestra.gestionObraCalidadEnsayoLaboratorioId,
                                                            fechaEntregaResultado:  muestra.fechaEntregaResultado !== undefined
                                                                                    ? new Date( muestra.fechaEntregaResultado ) : null,
                                                            nombreMuestra: muestra.nombreMuestra !== undefined ? muestra.nombreMuestra : '',
                                                            observacion: muestra.observacion !== undefined ? muestra.observacion : null,
                                                            seguimientoSemanalObservacionId,
                                                            tieneObservacionApoyo: muestra.tieneObservacionApoyo,
                                                            tieneObservacionSupervisor: muestra.tieneObservacionSupervisor,
                                                            fechaCreacion: obsApoyo.length > 0 ? obsApoyo[0].fechaCreacion : null,
                                                            fechaCreacionSupervisor: obsSupervisor.length > 0 ? obsSupervisor[0].fechaCreacion : null,
                                                            observacionApoyo: observacionApoyo !== undefined ? observacionApoyo : null,
                                                            observacionSupervisor: observacionSupervisor !== undefined ? observacionSupervisor : null,
                                                            dataHistorial: dataHistorial.length > 0 ? [ dataHistorial ] : [ [] ]

                                                        }
                                                    )
                                                );
                                            }
                                        );
                                }
                            }
                        }
                    );
            }
        );
    }

    getDataHistorial( dataHistorial: any[] ) {
        return new MatTableDataSource( dataHistorial );
    }

    crearFormulario() {
        this.formMuestra = this.fb.group(
            {
                muestras: this.fb.array( [] )
            }
        );
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

    guardar( muestra: FormGroup ) {
        console.log( muestra );
        if ( muestra.get( 'tieneObservacionSupervisor' ).value === false && muestra.get( 'observacionSupervisor' ).value !== null ) {
            muestra.get( 'observacionSupervisor' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: muestra.get( 'seguimientoSemanalObservacionId' ).value,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoMuestraEnsayo,
            observacionPadreId: muestra.get( 'ensayoLaboratorioMuestraId' ).value,
            observacion: muestra.get( 'observacionSupervisor' ).value,
            tieneObservacion: muestra.get( 'tieneObservacionSupervisor' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.muestras.clear();
                                this.getMuestras();
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
