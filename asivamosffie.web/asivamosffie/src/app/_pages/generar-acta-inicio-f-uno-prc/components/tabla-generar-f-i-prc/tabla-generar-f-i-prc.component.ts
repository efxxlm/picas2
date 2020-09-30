import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { CargarActaSuscritaActaIniFIPreconstruccionComponent } from '../cargar-acta-suscrita-acta-ini-f-i-prc/cargar-acta-suscrita-acta-ini-f-i-prc.component';
@Component({
  selector: 'app-tabla-generar-f-i-prc',
  templateUrl: './tabla-generar-f-i-prc.component.html',
  styleUrls: ['./tabla-generar-f-i-prc.component.scss']
})
export class TablaGenerarFIPreconstruccionComponent implements OnInit {
  displayedColumns: string[] = [ 'fechaAprobacionRequisitosSupervisor', 'numeroContrato', 'estadoActaContrato', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  public dataTable;
  constructor(private router: Router, public dialog: MatDialog, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.service.GetListContrato().subscribe(data=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    })

  }
  generarActaFUno(id){
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/generarActa',id]);
  }
  verDetalleEditarActaFUno(observaciones,id){
    if(observaciones == true){
      localStorage.setItem("conObservaciones","true");
    }
    else{
      localStorage.setItem("conObservaciones","false");
    }
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleEditarActa',id]);
  }
  enviarParaRevision(){
    alert("llama al servicio");
  }
  verDetalleActaFUno(observaciones,actaSuscrita,id){
    if(observaciones == true){
      localStorage.setItem("conObservaciones","true");
    }
    else{
      localStorage.setItem("conObservaciones","false");
    }
    if(actaSuscrita == true){
      localStorage.setItem("actaSuscrita","true");
    }
    else{
      localStorage.setItem("actaSuscrita","false");
    }
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa',id]);
  }
  enviarActaParaFirma(){
    alert("llama al servicio donde cambia estado a true");
  }
  cargarActaSuscrita(id){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '45%';
    dialogConfig.data = {id:id};
    const dialogRef = this.dialog.open(CargarActaSuscritaActaIniFIPreconstruccionComponent, dialogConfig);
  }
  descargarActaDesdeTabla(){
    alert("llama al servicio");
  }
}
