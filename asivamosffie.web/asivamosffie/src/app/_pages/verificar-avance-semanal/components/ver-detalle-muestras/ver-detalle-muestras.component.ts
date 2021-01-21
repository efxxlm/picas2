import { MatTableDataSource } from '@angular/material/table';
import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
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
        private dialog: MatDialog,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
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
                                    this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, muestra.ensayoLaboratorioMuestraId, tipoObservaciones.gestionCalidad.muestrasEnsayo )
                                        .subscribe(
                                            response => {
                                                const obsApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                                let observacionApoyo: string;
                                                let dataHistorial = [];
                                                let seguimientoSemanalObservacionId = 0;
                                                let semaforoMuestra: string;
                                                dataHistorial = response.filter( obs => obs.archivada === true );
                                                if ( obsApoyo.length > 0 ) {
                                                    seguimientoSemanalObservacionId = obsApoyo[0].seguimientoSemanalObservacionId;
                                                    if ( obsApoyo[0].observacion !== undefined ) {
                                                        if ( obsApoyo[0].observacion.length > 0 ) {
                                                            observacionApoyo = obsApoyo[0].observacion;
                                                        }
                                                    }
                                                }
                                                if ( muestra.registroCompletoObservacionApoyo === true ) {
                                                    semaforoMuestra = 'completo';
                                                }
                                                if (    muestra.registroCompletoObservacionApoyo === false ) {
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
                                                            registroCompletoObservacionApoyo: muestra.registroCompletoObservacionApoyo,
                                                            fechaCreacion: obsApoyo.length > 0 ? obsApoyo[0].fechaCreacion : null,
                                                            observacionApoyo: observacionApoyo !== undefined ? observacionApoyo : null,
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
        if ( muestra.get( 'tieneObservacionApoyo' ).value === false && muestra.get( 'observacionApoyo' ).value !== null ) {
            muestra.get( 'observacionApoyo' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: muestra.get( 'seguimientoSemanalObservacionId' ).value,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoMuestraEnsayo,
            observacionPadreId: muestra.get( 'ensayoLaboratorioMuestraId' ).value,
            observacion: muestra.get( 'observacionApoyo' ).value,
            tieneObservacion: muestra.get( 'tieneObservacionApoyo' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'False' )
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
