import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-control-y-tabla-actuacion-mt',
  templateUrl: './control-y-tabla-actuacion-mt.component.html',
  styleUrls: ['./control-y-tabla-actuacion-mt.component.scss']
})
export class ControlYTablaActuacionMtComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActualizacion',
    'actuacion',
    'numeroActuacion',
    'estadoRegistro',
    'estadoActuacion',
    'gestion',
  ];
  dataTable: any[] = [];  
  public idMesaTrabajo = parseInt(localStorage.getItem("idMesaTrabajo"));
  constructor(public dialog: MatDialog, private router: Router,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.services.GetActuacionesMesasByActuacionId(this.idMesaTrabajo).subscribe((data:any)=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    });
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }
  finalizarMesaDeTrabajo(id){
    this.services.SetStateActuacionMesa(id,'2').subscribe((resp:any)=>{
      if(resp.isSuccessful==true){
        this.ngOnInit();
      }
    });
  }
  verDetalleEditarMTActuacion(id,numActuacionMT){
    localStorage.setItem("numActuacionMT",numActuacionMT);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarMesaTrabajoAct',id]);
  }
  eliminarMTActuacion(id){
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",id);
  }
  verDetalleMTActuacion(id,numActuacionMT){
    localStorage.setItem("numActuacionMT",numActuacionMT);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleMesaTrabajoAct',id]);
  }
  deleteControversia(id) {
    this.services.EliminacionActuacionMesa(id).subscribe((resp:any)=>{
      if (resp.code == "301") {
        this.openDialog('', resp.message);
      }
      else{
        this.ngOnInit();
      }
    });
  }
  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.deleteControversia(e);
      }
    });
  }
}
