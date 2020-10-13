import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';
import { DialogCargarActaSuscritaConstComponent } from '../dialog-cargar-acta-suscrita-const/dialog-cargar-acta-suscrita-const.component';
/*
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
*/
@Component({
  selector: 'app-tabla-general-acta-fdos-const',
  templateUrl: './tabla-general-acta-fdos-const.component.html',
  styleUrls: ['./tabla-general-acta-fdos-const.component.scss']
})
export class TablaGeneralActaFdosConstComponent implements OnInit {
  displayedColumns: string[] = [ 'fechaAprobacionRequisitos', 'numeroContratoObra', 'estadoActa', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  public dataTable;
  constructor(private router: Router, public dialog: MatDialog, private services: ActBeginService) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.services.GetListGrillaActaInicio().subscribe(data=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
  }
  generarActaFDos(id){
    localStorage.setItem("editable","false");
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  verDetalleEditarActaFDos(id){
    localStorage.setItem("editable","true");
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
