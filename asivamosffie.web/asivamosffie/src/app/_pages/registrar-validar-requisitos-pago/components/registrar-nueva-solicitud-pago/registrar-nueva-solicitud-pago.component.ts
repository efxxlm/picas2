import { Dominio } from 'src/app/core/_services/common/common.service';
import { CommonService } from './../../../../core/_services/common/common.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogProyectosAsociadosComponent } from '../dialog-proyectos-asociados/dialog-proyectos-asociados.component';

@Component({
  selector: 'app-registrar-nueva-solicitud-pago',
  templateUrl: './registrar-nueva-solicitud-pago.component.html',
  styleUrls: ['./registrar-nueva-solicitud-pago.component.scss']
})
export class RegistrarNuevaSolicitudPagoComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
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

  addressForm = this.fb.group({
    tipoSolicitud: [null, Validators.required],
    modalidadContrato: [null, Validators.required],
    numeroContrato: [null, Validators.required],
  });
  tiposSolicitudArray: Dominio[] = [];
  tipoSolicitudCodigo: any = {};
  modalidadContratoArray = [
    { name: 'Tipo A', value: '1' },
    { name: 'Tipo B', value: '2' },
    { name: 'Mejoramiento', value: '3' }
  ];
  contratosArray = [
    { name: 'N801801', value: '1' }
  ];
  contratoId: any;
  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
     private commonSvc: CommonService )
  {
    this.commonSvc.tiposDeSolicitudes()
      .subscribe(
        solicitudes => {
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
          this.tiposSolicitudArray = solicitudes;
          console.log( this.tiposSolicitudArray, this.tipoSolicitudCodigo );
        }
      );
  }


  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  seleccionAutocomplete(id:any){
    this.addressForm.value.numeroContrato = id;
    this.contratoId = id;
  }
  openProyectosAsociados(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '1020px';
    //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(DialogProyectosAsociadosComponent, dialogConfig);
    //dialogRef.afterClosed().subscribe(value => {});
  }
}
