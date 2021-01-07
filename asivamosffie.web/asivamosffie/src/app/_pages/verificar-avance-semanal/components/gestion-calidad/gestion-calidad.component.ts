import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-calidad',
  templateUrl: './gestion-calidad.component.html',
  styleUrls: ['./gestion-calidad.component.scss']
})
export class GestionCalidadComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formGestionCalidad: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    formEnsayo: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    tablaHistorial = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [
        {
            fechaRevision: new Date(),
            responsable: 'Apoyo a la supervisión',
            historial: '<p>Se recomienda que en cada actividad se especifique el responsable.</p>'
        }
    ];
    seRealizoPeticion = false;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraCalidadId = 0;
    seguimientoSemanalObservacionId = 0;
    gestionObraCalidad: any;
    tipoEnsayos: any[] = [];
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
        private dialog: MatDialog,
        private routes: Router,
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    {
        this.commonSvc.listaTipoEnsayos()
            .subscribe( tipo => this.tipoEnsayos = tipo );
    }

    ngOnInit(): void {
        this.getGestionCalidad();
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
    }

    getGestionCalidad() {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
            if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad.length > 0 )
            {
                this.gestionObraCalidad = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad[0];
                if ( this.gestionObraCalidad.seRealizaronEnsayosLaboratorio !== undefined ) {
                    this.seguimientoSemanalGestionObraCalidadId = this.gestionObraCalidad.seguimientoSemanalGestionObraCalidadId;
                }
            }
        }
    }

    getTipoEnsayo( tipoEnsayoCodigo: string ) {
        if ( this.tipoEnsayos.length > 0 && this.gestionObraCalidad !== undefined ) {
            const tipoEnsayo = this.tipoEnsayos.filter( ensayo => ensayo.codigo === tipoEnsayoCodigo );
            return tipoEnsayo[0].nombre;
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
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    getVerDetalleMuestras( gestionObraCalidadEnsayoLaboratorioId: number ) {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleMuestras`, gestionObraCalidadEnsayoLaboratorioId ] );
    }

    guardar() {
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '9',
            observacionPadreId: this.seguimientoSemanalGestionObraId,
            observacion: this.formEnsayo.get( 'observaciones' ).value,
            tieneObservacion: this.formEnsayo.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarEnsayo( ObsEnsayoId?: number ) {
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: ObsEnsayoId !== undefined ? ObsEnsayoId : 0,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '10',
            observacionPadreId: this.seguimientoSemanalGestionObraCalidadId,
            observacion: this.formEnsayo.get( 'observaciones' ).value,
            tieneObservacion: this.formEnsayo.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
