import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';
import { TechnicalCommitteSessionService } from '../../../../core/_services/technicalCommitteSession/technical-committe-session.service';

@Component({
  selector: 'app-tabla-gestion-actas',
  templateUrl: './tabla-gestion-actas.component.html',
  styleUrls: ['./tabla-gestion-actas.component.scss']
})
export class TablaGestionActasComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  displayedColumns: string[] = [ 'fechaOrdenDia', 'numeroComite', 'estadoComiteCodigo', 'gestion' ];
  estadoActa = {
    revisarActa: '2',
    aprobado: '3',
    devuelto: '4'
  };

  constructor(
    private routes: Router,
    private compromisoSvc: CompromisosActasComiteService,
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
    private comiteTecnicoSvc: TechnicalCommitteSessionService ) { }

  ngOnInit(): void {
    this.getData();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  // getDataTabla
  getData() {

    this.compromisoSvc.getGrillaActas()
      .subscribe( ( resp: any[] ) => {
        const dataTable = [];

        resp.forEach( value => {
          if (  value.estadoActaCodigo === this.estadoActa.revisarActa
                || value.estadoActaCodigo === this.estadoActa.aprobado
                || value.estadoActaCodigo === this.estadoActa.devuelto ) 
          {
            dataTable.push( value );
          }
        } );

        this.dataSource = new MatTableDataSource( dataTable );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );

  }

  revisarActa( acta: any ) {
    this.routes.navigate( [ '/compromisosActasComite/revisionActa', acta.comiteTecnicoId ] );
  }

  getActaPdf( comiteTecnicoId: number , numeroComite: string ) {
    const esFiduciario = numeroComite.includes( 'F' );
    if ( esFiduciario === true ) {
      this.fiduciaryCommitteeSessionService.getPlantillaActaBySesionComiteSolicitudId(comiteTecnicoId)
        .subscribe(resp => {
          const documento = `Acta ${ numeroComite }.pdf`;
          const  blob = new Blob([resp], { type: 'application/pdf' });
          const  anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
          anchor.click();
        });
    }
    if ( esFiduciario === false ) {
      this.comiteTecnicoSvc.getPlantillaActaBySesionComiteSolicitudId( comiteTecnicoId )
        .subscribe( ( resp: any ) => {
          const documento = `Acta ${ numeroComite }.pdf`;
          const blob = new Blob([resp], { type: 'application/pdf' });
          const anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
          anchor.click();
        } );
    }
  }

}
