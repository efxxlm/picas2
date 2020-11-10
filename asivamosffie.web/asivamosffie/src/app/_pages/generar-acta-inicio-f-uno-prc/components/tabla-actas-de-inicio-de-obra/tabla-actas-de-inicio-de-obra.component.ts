import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { CargarActaSuscritaActaIniFIPreconstruccionComponent } from '../cargar-acta-suscrita-acta-ini-f-i-prc/cargar-acta-suscrita-acta-ini-f-i-prc.component';

export interface Contrato {
  fechaAprobacionRequisitos: string;
  numeroContrato:string;
  estado:string;
  enviadoparaInterventor:boolean;
  actaSuscrita:boolean;
}

const ELEMENT_DATA: Contrato[] = [
  {fechaAprobacionRequisitos:"20/06/2020",numeroContrato:"C223456789",estado:"Sin validar",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"21/06/2020",numeroContrato:"C223456790",estado:"Con observaciones",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"22/06/2020",numeroContrato:"C223456791",estado:"Con observaciones",enviadoparaInterventor:true,actaSuscrita:null},
  {fechaAprobacionRequisitos:"23/06/2020",numeroContrato:"C223456791",estado:"Enviada al interventor",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"24/06/2020",numeroContrato:"C223456792",estado:"Sin acta generada",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"25/06/2020",numeroContrato:"C223456793",estado:"Con acta generada",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"26/06/2020",numeroContrato:"C223456794",estado:"Con acta en proceso de firma",enviadoparaInterventor:null,actaSuscrita:null},
  {fechaAprobacionRequisitos:"27/06/2020",numeroContrato:"C223456795",estado:"Con acta suscrita y cargada",enviadoparaInterventor:null,actaSuscrita:true}
];
@Component({
  selector: 'app-tabla-actas-de-inicio-de-obra',
  templateUrl: './tabla-actas-de-inicio-de-obra.component.html',
  styleUrls: ['./tabla-actas-de-inicio-de-obra.component.scss']
})
export class TablaActasDeInicioDeObraComponent implements OnInit {
  displayedColumns: string[] = [ 'fechaAprobacionRequisitos', 'numeroContratoObra', 'estadoActa', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  public dataTable;
  constructor(private router: Router, public dialog: MatDialog, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.service.GetListGrillaActaInicio(8).subscribe(data=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
      this.applyFilter("Obra e Interventoria");
    });
  }
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue;
  }
  generarActaFUno(id){
    localStorage.setItem("origin", "obra");
    localStorage.setItem("editable", "false");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/validarActaDeInicio',id]);
  }
  verDetalleEditarActaFUno(observaciones,id){
    if(observaciones == true){
      localStorage.setItem("conObservaciones","true");
    }
    else{
      localStorage.setItem("conObservaciones","false");
    }
    localStorage.setItem("origin", "obra");
    localStorage.setItem("editable", "true");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/validarActaDeInicio',id]);
  }
  enviarParaRevision(idContrato, estadoActaContrato){
    estadoActaContrato="366";
    this.service.CambiarEstadoActa(idContrato,estadoActaContrato).subscribe(data=>{
      if(data.isSuccessful==true){
      }
    });
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
