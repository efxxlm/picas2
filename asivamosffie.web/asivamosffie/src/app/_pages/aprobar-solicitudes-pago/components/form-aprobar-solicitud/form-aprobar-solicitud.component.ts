import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosAprobComponent } from '../dialog-proyectos-asociados-aprob/dialog-proyectos-asociados-aprob.component';

@Component({
  selector: 'app-form-aprobar-solicitud',
  templateUrl: './form-aprobar-solicitud.component.html',
  styleUrls: ['./form-aprobar-solicitud.component.scss']
})
export class FormAprobarSolicitudComponent implements OnInit {

    contrato: any;
    idGestion: any;
    solicitud: string;
    tipoSolicitudCodigo: any = {};
    modalidadContratoArray: Dominio[] = [];
    tipoPagoArray: Dominio[] = [];
    addressForm: FormGroup;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    otrosCostosForm = this.fb.group({
        numeroContrato: [null, Validators.required],
        numeroRadicadoSAC: [null, Validators.required],
        numeroFactura: [null, Validators.required],
        valorFacturado: [null, Validators.required],
        tipoPago: [null, Validators.required],
        tieneObservaciones: [null, Validators.required],
        observaciones:[null, Validators.required]
    });
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
    displayedColumns: string[] = [
        'drp',
        'numDrp',
        'valor',
        'saldo'
    ];
    dataTable: any[] = [
        {
          drp: '1',
          numDrp: 'IP_00090',
          valor: '$100.000.000',
          saldo: '$100.000.000'
        },
        {
          drp: '2',
          numDrp: 'IP_00123',
          valor: '$5.000.000',
          saldo: '$5.000.000'
        }
    ];

    constructor(
        private activatedRoute: ActivatedRoute,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private fb: FormBuilder,
        private commonSvc: CommonService )
    {
        this.getContrato();
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
    }

    getContrato() {
        this.registrarPagosSvc.getContratoByContratoId( 13, 3 )
            .subscribe(
                response => {
                    this.commonSvc.tiposDeSolicitudes()
                        .subscribe(
                            solicitudes => {
                                this.commonSvc.modalidadesContrato()
                                    .subscribe( response => this.modalidadContratoArray = response );
                                for ( const solicitud of solicitudes ) {
                                    if ( solicitud.codigo === '1' ) {
                                      this.tipoSolicitudCodigo.contratoObra = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '2' ) {
                                      this.tipoSolicitudCodigo.contratoInterventoria = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '3' ) {
                                      this.tipoSolicitudCodigo.expensas = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '4' ) {
                                      this.tipoSolicitudCodigo.otrosCostos = solicitud.codigo;
                                    }
                                }
                                this.contrato = response;
                                console.log( this.contrato );
                                if ( this.contrato.solicitudPagoOnly.tipoSolicitudCodigo === this.tipoSolicitudCodigo.otrosCostos ) {
                                    this.commonSvc.tiposDePagoExpensas()
                                    .subscribe( response => {
                                        this.tipoPagoArray = response;
                                        if ( this.contrato !== undefined ) {
                                            const solicitudPagoOtrosCostosServicios = this.contrato.solicitudPagoOnly.solicitudPagoOtrosCostosServicios[0];
                                            this.otrosCostosForm.setValue(
                                                {
                                                    numeroContrato: this.contrato.numeroContrato,
                                                    numeroRadicadoSAC: solicitudPagoOtrosCostosServicios.numeroRadicadoSac !== undefined ? solicitudPagoOtrosCostosServicios.numeroRadicadoSac : null,
                                                    numeroFactura: solicitudPagoOtrosCostosServicios.numeroFactura !== undefined ? solicitudPagoOtrosCostosServicios.numeroFactura : null,
                                                    valorFacturado: solicitudPagoOtrosCostosServicios.valorFacturado !== undefined ? solicitudPagoOtrosCostosServicios.valorFacturado : null,
                                                    tipoPago: solicitudPagoOtrosCostosServicios.tipoPagoCodigo !== undefined ? this.tipoPagoArray.filter( tipoPago => tipoPago.codigo === solicitudPagoOtrosCostosServicios.tipoPagoCodigo )[0] : null
                                                }
                                            );
                                        }
                                    } );
                                } else {
                                    this.dataSource = new MatTableDataSource( this.contrato.contratacion.disponibilidadPresupuestal );
                                    this.dataSource.paginator = this.paginator;
                                    this.dataSource.sort = this.sort;
                                }
                            }
                        );
                }
            );
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
            return modalidad[0].nombre;
        }
    }

    crearFormulario() {
        return this.fb.group({
          tieneObservaciones: [null, Validators.required],
          observaciones:[null, Validators.required]
        })
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
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

    openProyectosAsociados() {
        if ( this.contrato === undefined ) {
            return;
        }

        const dialogRef = this.dialog.open( DialogProyectosAsociadosAprobComponent, {
            width: '80em',
            data: { contrato: this.contrato }
        });
    }

    onSubmit() {
        console.log(this.addressForm.value);
    }

}


