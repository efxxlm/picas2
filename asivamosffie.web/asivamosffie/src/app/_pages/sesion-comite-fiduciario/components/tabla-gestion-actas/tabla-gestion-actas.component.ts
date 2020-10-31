import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ComiteGrilla, ComiteTecnico, EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';

export interface OrdenDelDia {
  id: number;
  fecha: string;
  numero: string;
  estadoAprobacion: string;
  estadoRegistro: string;
}

const ELEMENT_DATA: OrdenDelDia[] = [
  { id: 0, fecha: '24/06/2020', numero: 'CF_00001', estadoAprobacion: 'Sin acta', estadoRegistro: 'Incompleto' }
];

@Component({
  selector: 'app-tabla-gestion-actas',
  templateUrl: './tabla-gestion-actas.component.html',
  styleUrls: ['./tabla-gestion-actas.component.scss']
})
export class TablaGestionActasComponent implements OnInit {

  estadosComite = EstadosComite;

  displayedColumns: string[] = ['fecha', 'numero', 'estadoAprobacion', 'estadoRegistro', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
                public dialog: MatDialog,
                
             ) 
  {

  }

  ngOnInit(): void {

    this.fiduciaryCommitteeSessionService.getCommitteeSession()
      .subscribe( response => {
        let lista: ComiteGrilla[] = response.filter( c => [EstadosComite.desarrolladaSinActa, 
                                                          EstadosComite.conActaDeSesionEnviada,
                                                          EstadosComite.conActaDeSesionAprobada].includes( c.estadoComiteCodigo ) )
        this.dataSource = new MatTableDataSource( lista );
      })

      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
        if (length === 0 || pageSize === 0) {
          return '0 de ' + length;
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;
        // If the start index exceeds the list length, do not try and fix the end index to the end.
        const endIndex = startIndex < length ?
          Math.min(startIndex + pageSize, length) :
          startIndex + pageSize;
        return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
      };
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  enviarSolicitud( id: number ){
    let comite: ComiteTecnico = {
      comiteTecnicoId: id,
      estadoComiteCodigo: EstadosComite.conActaDeSesionEnviada,sesionComiteSolicitudComiteTecnicoFiduciario:null
 
    }
    this.fiduciaryCommitteeSessionService.enviarComiteParaAprobacion( comite )
    .subscribe( respuesta => {
      this.openDialog( '', `<b>${respuesta.message}</b>`);
      if ( respuesta.code == "200" )
        this.ngOnInit();  
    })
  }

  descargarActa( id: number ){
    this.fiduciaryCommitteeSessionService.getPlantillaActaBySesionComiteSolicitudId(id)
      .subscribe(resp => {
        console.log(resp);
        const documento = `ActaComiteFiduciario ${ id }.pdf`;
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
