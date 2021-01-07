import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-sst',
  templateUrl: './gestion-sst.component.html',
  styleUrls: ['./gestion-sst.component.scss']
})
export class GestionSstComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalObservacionId = 0;
    gestionObraSst: any;
    formGestionSst: FormGroup = this.fb.group({
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
    causasDeAccidentes: Dominio[] = [];

    constructor(
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private fb: FormBuilder,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private routes: Router )
    { }

    ngOnInit(): void {
        this.getGestionSst();
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
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

    getGestionSst() {
        this.commonSvc.listaCausaAccidente()
        .subscribe( response => {
            this.causasDeAccidentes = response;
            if ( this.seguimientoSemanal !== undefined ) {
                this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
                this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;

                if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                    && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud.length > 0 )
                {
                    this.gestionObraSst =
                        this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud[0];
                }
            }
        } );
    }

    getCausasDetalle( causas: any[] ) {
        if ( this.causasDeAccidentes.length > 0 && this.seguimientoSemanal !== undefined ) {
            const causaSeleccion = [];
            causas.forEach( causa => {
                this.causasDeAccidentes.filter( value => {
                    if ( causa.causaAccidenteCodigo === value.codigo ) {
                        causaSeleccion.push( value );
                    }
                } );
            } );
            return causaSeleccion;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        console.log( this.formGestionSst.value );
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '12',
            observacionPadreId: this.seguimientoSemanalGestionObraId,
            observacion: this.formGestionSst.get( 'observaciones' ).value,
            tieneObservacion: this.formGestionSst.get( 'tieneObservaciones' ).value,
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
