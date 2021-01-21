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
  esSupervisor: boolean;
  displayedColumns: string[] = [ 'fechaAprobacionRequisitos', 'numeroContratoObra', 'estadoActa', 'contratoId'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  dataTable: any[] = [];
  constructor(private router: Router, public dialog: MatDialog, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.service.GetListGrillaActaInicio(8).subscribe((data:any)=>{
      for (let contrObras of data) {
        if (contrObras.tipoContratoNombre === 'Obra' && (contrObras.estadoActaCodigo!='13' && contrObras.estadoActaCodigo!='14')) {
          this.dataTable.push(contrObras);
        };
      };
      console.log(this.dataTable);
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
  }
  cargarRol() {
    const userRol = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    //jflorez, el perfil 11 es interventor.....
    if (userRol == 11) {
      this.esSupervisor = false;
    }
    else {
      this.esSupervisor = true;
    }
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  validarActaParaInicio(id){
    localStorage.setItem("origin","obra");
    localStorage.setItem("editable","false");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/validarActaDeInicio',id]);
  }
  verDetalleEditar(id){
    localStorage.setItem("origin","obra");
    localStorage.setItem("editable","true");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/validarActaDeInicio',id]);
  }
  verDetalle(id){
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/verDetalleActa',id]);
  }
  generarActaFDos(){
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion/generarActa']);
  }
  enviarRevision(id,estadoObs){
    if(estadoObs=="Con revisión sin observaciones"){
      this.service.CambiarEstadoActa(id,"18").subscribe(data=>{
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
        );
      });
    }
    else{
      this.service.CambiarEstadoActa(id,"17").subscribe(data=>{
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
        );
      });
    }
    /*this.service.EnviarCorreoSupervisorContratista(id,2).subscribe(resp=>{
    });*/
  }
  enviarInterventor(id){
    if(localStorage.getItem("estadoObs")=="Con revisión sin observaciones"){
      this.service.CambiarEstadoActa(id,"18").subscribe(data=>{
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
        );
      });
    }
    else{
      this.service.CambiarEstadoActa(id,"17").subscribe(data=>{
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/generarActaInicioFaseIPreconstruccion'])
        );
      });
    }
  }
  cargarActaSuscrita(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    const dialogRef = this.dialog.open(CargarActaSuscritaActaIniFIPreconstruccionComponent, dialogConfig);
  }
  descargarActaDesdeTabla(id, numContrato){
    this.service.GetActaByIdPerfil(id, 'False').subscribe(resp => {
      const documento = `Acta contrato ${numContrato}.pdf`; // Valor de prueba
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
