import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DialogCargarActaSuscritaConstComponent } from '../dialog-cargar-acta-suscrita-const/dialog-cargar-acta-suscrita-const.component';

export interface PeriodicElement {
  fechaAprobacionRequisitosSupervisor: string;
  numeroContrato: string;
  estadoActaContrato: string;
  contratoId: number;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {contratoId: 1, fechaAprobacionRequisitosSupervisor: '20/06/2020', estadoActaContrato: "Sin acta generada", numeroContrato: 'C326326'},
  {contratoId: 2, fechaAprobacionRequisitosSupervisor: '21/06/2020', estadoActaContrato: "Con acta preliminar generada", numeroContrato: 'A208208'},
  {contratoId: 3, fechaAprobacionRequisitosSupervisor: '22/06/2020', estadoActaContrato: "Con acta en proceso de firma", numeroContrato: 'C801801'},
];
@Component({
  selector: 'app-tabla-general-acta-fdos-const',
  templateUrl: './tabla-general-acta-fdos-const.component.html',
  styleUrls: ['./tabla-general-acta-fdos-const.component.scss']
})
export class TablaGeneralActaFdosConstComponent implements OnInit {
  displayedColumns: string[] = [ 'fechaAprobacionRequisitosSupervisor', 'numeroContrato', 'estadoActaContrato', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(private router: Router, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.dataSource = new MatTableDataSource(ELEMENT_DATA);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }
  generarActaFDos(id){
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  verDetalleEditarActaFDos(observaciones,id){
    if(observaciones == true){
      localStorage.setItem("conObservaciones","true");
    }
    else{
      localStorage.setItem("conObservaciones","false");
    }
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  enviarParaRevision(idContrato, estadoActaContrato){
    /*estadoActaContrato="366";
    this.service.CambiarEstadoActa(idContrato,estadoActaContrato).subscribe(data=>{
      if(data.isSuccessful==true){
        this.showSendForRevisionBtn=false;
      }
    });*/
  }
  verDetalleActaFDos(id){
    this.router.navigate(['/generarActaInicioConstruccion/verDetalleActaConstruccion',id]);
  }
  enviarActaParaFirma(){
    alert("llama al servicio donde cambia estado a true");
  }
  cargarActaSuscrita(id){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '45%';
    dialogConfig.data = {id:id};
    const dialogRef = this.dialog.open(DialogCargarActaSuscritaConstComponent, dialogConfig);
  }
  descargarActaDesdeTabla(){
    alert("llama al servicio");
  }
}
