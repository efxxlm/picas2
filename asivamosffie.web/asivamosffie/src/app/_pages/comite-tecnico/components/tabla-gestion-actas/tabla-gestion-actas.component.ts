import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ComiteGrilla, ComiteTecnico, EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';

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
    private technicalCommitteeSessionService: TechnicalCommitteSessionService,
    public dialog: MatDialog,
  ) {

  }

  ngOnInit(): void {

    this.technicalCommitteeSessionService.getListComiteGrilla()
      .subscribe(response => {
        response.forEach(element => {
          element.fechaComite = element.fechaComite.split('T')[0].split('-').reverse().join('/');
        });
        const lista: ComiteGrilla[] = response.filter(c => [EstadosComite.desarrolladaSinActa,
        EstadosComite.conActaDeSesionEnviada,
        EstadosComite.conActaDeSesionAprobada].includes(c.estadoComiteCodigo));
        this.dataSource = new MatTableDataSource(lista);
        this.initPaginator();
      });
  }
  initPaginator() {
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

  enviarSolicitud(id: number) {
    const comite: ComiteTecnico = {
      comiteTecnicoId: id,
      estadoComiteCodigo: EstadosComite.conActaDeSesionEnviada,

    };
    this.technicalCommitteeSessionService.enviarComiteParaAprobacion(comite)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if (respuesta.code === '200') {
          this.ngOnInit();
        }
      });
  }

  descargarActa(id: number) {
    this.technicalCommitteeSessionService.getPlantillaActaBySesionComiteSolicitudId(id)
      .subscribe(resp => {
        console.log(resp);
        const documento = `ActaComiteTecnico ${id}.pdf`;
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
