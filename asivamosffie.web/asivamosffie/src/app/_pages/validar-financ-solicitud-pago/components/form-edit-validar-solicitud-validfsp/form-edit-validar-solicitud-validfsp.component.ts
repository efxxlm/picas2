import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { DialogProyectosAsociadosValidfspComponent } from '../dialog-proyectos-asociados-validfsp/dialog-proyectos-asociados-validfsp.component';

@Component({
  selector: 'app-form-edit-validar-solicitud-validfsp',
  templateUrl: './form-edit-validar-solicitud-validfsp.component.html',
  styleUrls: ['./form-edit-validar-solicitud-validfsp.component.scss']
})
export class FormEditValidarSolicitudValidfspComponent implements OnInit {
  idGestion: any;
  solicitud: string;
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
  constructor(private activatedRoute: ActivatedRoute, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idGestion = param.id;
      this.loadData(param.id);
    });
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  loadData(id){
    this.idGestion=id;
    //Prueba de versión maquetada
    if(this.idGestion==1){
      this.solicitud = 'SolPagoO0001';
    }
    else{
      this.solicitud = 'SolPagoEspecialO0001';
    }
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  openProyectosAsociados(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '1020px';
    //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(DialogProyectosAsociadosValidfspComponent, dialogConfig);
    //dialogRef.afterClosed().subscribe(value => {});
  }

}
