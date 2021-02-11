import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-obs-registrar-solicitud-pago',
  templateUrl: './obs-registrar-solicitud-pago.component.html',
  styleUrls: ['./obs-registrar-solicitud-pago.component.scss']
})
export class ObsRegistrarSolicitudPagoComponent implements OnInit {

    @Input() contrato: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any;
    @Input() registrarSolicitudPago: any;
    @Output() estadoSemaforo = new EventEmitter<string>();
    solicitudPagoObservacionId = 0;
    solicitudPago: any;
    solicitudPagoFase: any;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'faseContrato',
        'pagosRealizados',
        'valorFacturado',
        'porcentajeFacturado',
        'saldoPorPagar',
        'porcentajePorPagar'
    ];
    dataTable: any[] = [
        {
          faseContrato: 'Fase 1 - Preconstrucción',
          pagosRealizados: '0',
          valorFacturado: '0',
          porcentajeFacturado: '0',
          saldoPorPagar: '$30.000.000',
          porcentajePorPagar: '100%',
        },
        {
          faseContrato: 'Fase 2 - Construcción',
          pagosRealizados: '0',
          valorFacturado: '0',
          porcentajeFacturado: '0',
          saldoPorPagar: '$75.000.000',
          porcentajePorPagar: '100%',
        }
    ];
    addressForm: FormGroup;
    editorStyle = {
      height: '45px',
      overflow: 'auto'
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
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.contrato !== undefined ) {
            this.solicitudPago = this.contrato.solicitudPagoOnly;
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( this.aprobarSolicitudPagoId, this.solicitudPago.solicitudPagoId, this.solicitudPagoFase.solicitudPagoFaseId )
                .subscribe(
                    response => {
                        const obsSupervisor = response.filter( obs => obs.archivada === false )[0];
                        
                        if ( obsSupervisor !== undefined ) {
                            console.log( obsSupervisor );
                            this.solicitudPagoObservacionId = obsSupervisor.solicitudPagoObservacionId;
                            this.addressForm.setValue(
                                {
                                    fechaCreacion: obsSupervisor.fechaCreacion,
                                    tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                    observaciones: obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null
                                }
                            );
                        }
                    }
                );
        }

        this.dataSource = new MatTableDataSource(this.dataTable);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    };

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    crearFormulario() {
      return this.fb.group({
        fechaCreacion: [ null ],
        tieneObservaciones: [null, Validators.required],
        observaciones:[null, Validators.required],
      })
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

    onSubmit() {   
        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.registrarSolicitudPago.registrarSolicitudPagoCodigo,
            menuId: this.aprobarSolicitudPagoId,
            idPadre: this.solicitudPagoFase.solicitudPagoFaseId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => this.openDialog( '', `<b>${ response.message }</b>` ),
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
