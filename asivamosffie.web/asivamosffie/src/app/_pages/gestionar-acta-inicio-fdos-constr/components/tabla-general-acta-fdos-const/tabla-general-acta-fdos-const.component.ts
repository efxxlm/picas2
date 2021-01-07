import { GestionarActPreConstrFUnoService } from './../../../../core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
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
  dataTable: any[]= [];
  loadDataItems: Subscription;
  @Input () dataTableAux;
  constructor(private router: Router, public dialog: MatDialog, private services: ActBeginService, private gestionarActa: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.dataTableAux = this.dataTable;
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.services.GetListGrillaActaInicio(2).subscribe((data:any)=>{
      console.log( data );
      for (let actas of data){
        if(actas.tipoContrato == 'Obra'){
          this.dataTable.push(actas);
        }
      }
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
  }
  generarActaFDos(id,tipoContrato){
    if(tipoContrato=='Interventoria'){
      localStorage.setItem("origin", "interventoria");
    }
    else{
      localStorage.setItem("origin","obra");
    }
    localStorage.setItem("editable","false");
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  verDetalleEditarActaFDos(id,tipoContrato){
    if(tipoContrato=='Interventoria'){
      localStorage.setItem("origin", "interventoria");
    }
    else{
      localStorage.setItem("origin","obra");
    }
    localStorage.setItem("editable","true");
    this.router.navigate(['/generarActaInicioConstruccion/generarActaFDos',id]);
  }
  enviarParaRevision(idContrato){
    this.services.CambiarEstadoActa(idContrato,"21","usr2").subscribe(data=>{
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioConstruccion'])
      );
    });
  }
  verDetalleActaFDos(id){
    this.router.navigate(['/generarActaInicioConstruccion/verDetalleActaConstruccion',id]);
  }
  enviarActaParaFirma(id){
    this.descargarActaDesdeTabla(id);
    this.services.CambiarEstadoActa(id,"19","usr2").subscribe(data=>{
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioConstruccion'])
      );
    });
  }
  enviarRevisionAprobacionInt(id){
    this.services.CambiarEstadoActa(id,"3","usr2").subscribe(data=>{
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioConstruccion'])
      );
    });
  }
  enviarRevisionAprobacionTecEst2(id){
    this.services.CambiarEstadoActa(id,"2","usr2").subscribe(data=>{
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['/generarActaInicioConstruccion'])
      );
    });
  }
  verDetalleActaCargada(id){
    localStorage.setItem("actaSuscrita","true");
    this.router.navigate(['/generarActaInicioConstruccion/verDetalleActaConstruccion',id]);
  }
  cargarActaSuscrita(id,tipoContrato,numContrato){
    let idRol = 2; 
    let fecha1Titulo;
    let fecha2Titulo;
    if(tipoContrato=='Interventoria'){
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de interventoría';
      fecha2Titulo = 'Fecha de la firma del documento por parte del supervisor';
    }
    else{
      fecha1Titulo = 'Fecha de la firma del documento por parte del contratista de obra';
      fecha2Titulo = 'Fecha de la firma del documento por parte del contratista de interventoría';
    }
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    dialogConfig.data = {id:id, idRol:idRol, numContrato:numContrato, fecha1Titulo:fecha1Titulo, fecha2Titulo:fecha2Titulo};
    const dialogRef = this.dialog.open(DialogCargarActaSuscritaConstComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(value => {
      if (value == 'aceptado') {
        if(tipoContrato=='Obra'){
          this.services.CambiarEstadoActa(id,"20","usr2").subscribe(data=>{
            this.router.navigateByUrl('/', { skipLocationChange: true }).then(
              () => this.router.navigate(['/generarActaInicioConstruccion'])
            );
          });
        }
        else{
          this.services.CambiarEstadoActa(id,"7","usr2").subscribe(data0=>{
            this.router.navigateByUrl('/', { skipLocationChange: true }).then(
              () => this.router.navigate(['/generarActaInicioConstruccion'])
            );
          });
        }
      }
    });
  }
  descargarActaDesdeTabla(id, numContrato?){
    this.gestionarActa.GetActaByIdPerfil(id, 'True').subscribe(resp=>{
      const documento = `${ numContrato }.pdf`; // Valor de prueba
      const text = documento,
      blob = new Blob([resp], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
}
