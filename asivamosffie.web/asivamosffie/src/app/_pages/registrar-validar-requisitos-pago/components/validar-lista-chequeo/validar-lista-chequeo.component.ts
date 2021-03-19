import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { DialogObservacionesItemListchequeoComponent } from '../dialog-observaciones-item-listchequeo/dialog-observaciones-item-listchequeo.component';
import { DialogSubsanacionComponent } from '../dialog-subsanacion/dialog-subsanacion.component';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-validar-lista-chequeo',
  templateUrl: './validar-lista-chequeo.component.html',
  styleUrls: ['./validar-lista-chequeo.component.scss']
})
export class ValidarListaChequeoComponent implements OnInit {

    @Input() contrato: any;
    @Input() esVerDetalle = false;
    solicitudPagoModificado: any;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'item',
      'documento',
      'revTecnica',
      'observaciones'
    ];
    listaRevisionTecnica: Dominio[] = [];
    noCumpleCodigo = '2';
    seDiligencioCampo = false;

    constructor(
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private dialog: MatDialog,
        private commonSvc: CommonService )
    {
        this.commonSvc.listaRevisionTecnica()
            .subscribe( listaRevisionTecnica => this.listaRevisionTecnica = listaRevisionTecnica );
    }

    ngOnInit(): void {
        
        for ( const solicitudPagoListaChequeo of this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo ) {
            for ( const solicitudPagoListaChequeoRespuesta of solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta ) {
                solicitudPagoListaChequeoRespuesta.respuestaCodigo = solicitudPagoListaChequeoRespuesta.respuestaCodigo !== undefined ? solicitudPagoListaChequeoRespuesta.respuestaCodigo : null;
                solicitudPagoListaChequeoRespuesta.observacion = solicitudPagoListaChequeoRespuesta.observacion !== undefined ? solicitudPagoListaChequeoRespuesta.observacion : null;
            }
        }
        this.solicitudPagoModificado = this.contrato.solicitudPagoOnly;
        console.log( this.solicitudPagoModificado );
        this.dataSource = new MatTableDataSource();
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getMatTable( solicitudPagoListaChequeoRespuesta: any[] ) {
        return new MatTableDataSource( solicitudPagoListaChequeoRespuesta );
    }

    getRevisionTecnica( respuestaCodigo: string ) {
        if ( this.listaRevisionTecnica.length > 0 ) {
            const revision = this.listaRevisionTecnica.find( revision => revision.codigo === respuestaCodigo );

            if ( revision !== undefined ) {
                return revision.nombre;
            };
        }
    }

    getObservacion( registro: any, index: number, jIndex: number ) {
        const dialogRef = this.dialog.open(DialogObservacionesItemListchequeoComponent, {
            width: '70em',
            data: { contrato: this.contrato, registro, jIndex, esVerDetalle: this.esVerDetalle }
        });

        dialogRef.afterClosed()
            .subscribe(
                obs => {
                    this.solicitudPagoModificado.solicitudPagoListaChequeo[ index ].solicitudPagoListaChequeoRespuesta[ jIndex ].observacion = obs;
                }
            );
    }

    callObservaciones(){
      const dialogConfig = new MatDialogConfig();
      dialogConfig.height = 'auto';
      dialogConfig.width = '865px';
      //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
      const dialogRef = this.dialog.open(DialogObservacionesItemListchequeoComponent, dialogConfig);
      //dialogRef.afterClosed().subscribe(value => {});
    }

    getEstadoSemaforo( solicitudPagoListaChequeo: any ) {

        let sinDiligenciar = 0;

        solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta.forEach( value => {
            if ( value.respuestaCodigo === null ) {
                sinDiligenciar++;
            }
        } );

        if ( sinDiligenciar === solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta.length ) {
            return 'sin-diligenciar';
        }
        if ( solicitudPagoListaChequeo.registroCompleto === false ) {
            return 'en-proceso';
        }
        if ( solicitudPagoListaChequeo.registroCompleto === true ) {
            return 'completo';
        }
    }

    callSubsanacion(){
      const dialogConfig = new MatDialogConfig();
      dialogConfig.height = 'auto';
      dialogConfig.width = '865px';
      //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
      const dialogRef = this.dialog.open(DialogSubsanacionComponent, dialogConfig);
      //dialogRef.afterClosed().subscribe(value => {});
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    disabledBtn() {
        this.seDiligencioCampo = true;
    }

    guardar() {
        this.registrarPagosSvc.createEditNewPayment( this.solicitudPagoModificado )
        .subscribe(
            response => {
                this.openDialog( '', `<b>${ response.message }</b>` );
                this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPagoModificado.solicitudPagoId )
                    .subscribe(
                        () => {
                            this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                () => this.routes.navigate(
                                    [
                                        '/registrarValidarRequisitosPago/verDetalleEditar',  this.solicitudPagoModificado.contratoId, this.solicitudPagoModificado.solicitudPagoId
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
