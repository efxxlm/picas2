import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-control-y-tabla-cc-general',
  templateUrl: './control-y-tabla-cc-general.component.html',
  styleUrls: ['./control-y-tabla-cc-general.component.scss']
})
export class ControlYTablaCcGeneralComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaSolicitud',
    'numeroSolicitud',
    'numeroContrato',
    'tipoControversia',
    'estadoControversia',
    'gestion',
  ];
  public dataTable;
  constructor(public dialog: MatDialog, private router: Router, private services: ContractualControversyService) {
  }
  ngOnInit(): void {
    this.services.GetListGrillaTipoSolicitudControversiaContractual().subscribe(data => {
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
    /*
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    */
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  verDetalleEditarTramiteButton(id) {
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarControversia', id]);
  }

  deleteControversia(id) {
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",id);
  }
  deleteControversiaConfirmed(id){
    this.services.EliminarControversiaContractual(id).subscribe((dataEliminado: any) => {
      if (dataEliminado.isSuccessful == true) {
        this.ngOnInit();
      }
    });
  }
  sendTramiteToComite(id) {
    this.services.CambiarEstadoControversiaContractual(id, "5").subscribe((dataUpdt: any) => {
      if (dataUpdt.isSuccessful == true) {
        this.ngOnInit();
      }
    });
  }
  actualizarTramiteButton(id) {
    localStorage.setItem("controversiaID", id);
    this.services.CambiarEstadoControversiaContractual(id, "10").subscribe((dataUpdt: any) => {
      this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
    });
  }
  consultarActualizaciones(id){
    localStorage.setItem("controversiaID", id);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
  }
  verDetalleButton(id) {
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleControversia', id]);
  }
  descargarProcesoControversia(id){
    this.services.GetPlantillaControversiaContractual(id).subscribe((esPdf:any)=>{
      const documento = `Prueba.pdf`; // Valor de prueba
      const text = documento,
      blob = new Blob([esPdf], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
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
        this.deleteControversiaConfirmed(e);
      }
    });
  }
}
