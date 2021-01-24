import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-comite-obra',
  templateUrl: './comite-obra.component.html',
  styleUrls: ['./comite-obra.component.scss']
})
export class ComiteObraComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoComiteObra: any;
    numeroComiteObra: string;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistrarComiteObraId: number;
    gestionComiteObra: any;
    seguimientoSemanalObservacionId = 0;
    observacionApoyo: any[] = [];
    tablaHistorial = new MatTableDataSource();
    formComiteObra: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
	displayedColumnsHistorial: string[]  = [
		'fechaRevision',
		'responsable',
		'historial'
	];
	dataHistorial: any[] = [];
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
            this.seguimientoSemanalRegistrarComiteObraId =  this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0].seguimientoSemanalRegistrarComiteObraId : 0;
            if ( this.seguimientoSemanal.comiteObraGenerado !== undefined ) {
                this.numeroComiteObra = this.seguimientoSemanal.comiteObraGenerado;
            }

            if ( this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ) {
                this.gestionComiteObra = this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0];
                this.numeroComiteObra = this.gestionComiteObra.numeroComite;
                if ( this.gestionComiteObra.observacionApoyoId !== undefined || this.gestionComiteObra.observacionSupervisorId !== undefined ) {
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalRegistrarComiteObraId, this.tipoComiteObra )
                        .subscribe(
                            response => {
                                this.observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const observacionSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.dataHistorial = response.filter( obs => obs.archivada === true );
                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                if ( observacionSupervisor.length > 0 ) {
                                    if ( observacionSupervisor[0].observacion !== undefined ) {
                                        if ( observacionSupervisor[0].observacion.length > 0 ) {
                                            this.formComiteObra.get( 'observaciones' ).setValue( observacionSupervisor[0].observacion );
                                        }
                                    }
                                    this.seguimientoSemanalObservacionId = observacionSupervisor[0].seguimientoSemanalObservacionId;
                                    this.formComiteObra.get( 'tieneObservaciones' ).setValue( this.gestionComiteObra.tieneObservacionSupervisor );
                                    this.formComiteObra.get( 'fechaCreacion' ).setValue( observacionSupervisor[0].fechaCreacion );
                                }
                            }
                        );
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
        if ( this.formComiteObra.get( 'tieneObservaciones' ).value === false && this.formComiteObra.get( 'observaciones' ).value !== null ) {
            this.formComiteObra.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoComiteObra,
            observacionPadreId: this.seguimientoSemanalRegistrarComiteObraId,
            observacion: this.formComiteObra.get( 'observaciones' ).value,
            tieneObservacion: this.formComiteObra.get( 'tieneObservaciones' ).value,
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
