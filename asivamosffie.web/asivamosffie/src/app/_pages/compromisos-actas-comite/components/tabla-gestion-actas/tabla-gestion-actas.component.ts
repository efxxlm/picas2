import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';

@Component({
  selector: 'app-tabla-gestion-actas',
  templateUrl: './tabla-gestion-actas.component.html',
  styleUrls: ['./tabla-gestion-actas.component.scss']
})
export class TablaGestionActasComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  displayedColumns: string[] = [ 'fechaCreacion', 'numeroComite', 'estadoComiteCodigo', 'gestion' ];
  estadoCodigo: string;

  constructor ( private routes: Router,
                private compromisoSvc: CompromisosActasComiteService ) { }

  ngOnInit(): void {
    this.getData();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  //getDataTabla
  getData () {

    this.compromisoSvc.getGrillaActas()
      .subscribe( ( resp: any ) => {
        this.estadoCodigo = resp.estadoComiteCodigo;
        console.log( resp );
        this.dataSource = new MatTableDataSource( resp );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );

  }

  revisarActa ( acta: any ) {
    this.routes.navigate( [ '/compromisosActasComite/revisionActa', acta.comiteTecnicoId ] )
  }

  getActaPdf ( comiteTecnicoId: number , numeroComite: string ) {
    this.compromisoSvc.getActaPdf( comiteTecnicoId )
      .subscribe( ( resp: any ) => {

        const documento = `Acta ${ numeroComite }.pdf`;
        const text = documento,
        blob = new Blob([resp], { type: 'application/pdf' }),
        anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
        
      } )
  }

}
