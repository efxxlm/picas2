import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-menu-ficha',
  templateUrl: './menu-ficha.component.html',
  styleUrls: ['./menu-ficha.component.scss']
})
export class MenuFichaComponent implements OnInit {
  @Input() indicadores: any;

  constructor() { }

  ngOnInit(): void {
  }

}
