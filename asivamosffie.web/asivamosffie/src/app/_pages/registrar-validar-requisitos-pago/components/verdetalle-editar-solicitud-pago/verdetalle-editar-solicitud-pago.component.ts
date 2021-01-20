import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosComponent } from '../dialog-proyectos-asociados/dialog-proyectos-asociados.component';

@Component({
  selector: 'app-verdetalle-editar-solicitud-pago',
  templateUrl: './verdetalle-editar-solicitud-pago.component.html',
  styleUrls: ['./verdetalle-editar-solicitud-pago.component.scss']
})
export class VerdetalleEditarSolicitudPagoComponent implements OnInit {

    contrato: any;
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

  constructor(
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog,
    private registrarPagosSvc: RegistrarRequisitosPagoService,
    private commonSvc: CommonService )
  {
    this.registrarPagosSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
        .subscribe(
            response => {
                this.contrato = response;
                console.log( this.contrato );
            }
        );
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {

    });
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  openProyectosAsociados(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(DialogProyectosAsociadosComponent, dialogConfig);
    //dialogRef.afterClosed().subscribe(value => {});
  }

}
