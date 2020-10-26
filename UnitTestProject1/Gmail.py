# the following line imports all the dragonfly stuff we'll be using -- obviously you must have Dragonfly installed
from dragonfly import *
import _webdriver_utils as webdriver


# now we make our MappingRule object with only two commands
class Gmail(MappingRule):
  mapping = {
    "open": Key("plus, o"),
    "archive": Key("+, {"),
    "done": Key("+, ["),
    "this unread": Key("+, _"),
    "undo": Key("plus, z"),
    "list": Key("plus, u"),
    "compose": Key("plus, c"),
    "reply": Key("plus, r"),
    "reply all": Key("plus, a"),
    "forward": Key("plus, f"),
    "important": Key("plus, plus"),
    "this star": Key("plus, s"),
    "this important": Key("plus, plus"),
    "this not important": Key("plus, minus"),
    "label waiting": Pause("50") + Key("plus, l/50") + Text("waiting") + Pause("50") + Key("enter"),
    "label snooze": Pause("50") + Key("plus, l/50") + Text("snooze") + Pause("50") + Key("enter"),
    "snooze": Pause("50") + Key("plus, l/50") + Text("snooze") + Pause("50") + Key("enter") + Pause("50") + Key("+, ["),
    "label vacation": Pause("50") + Key("plus, l/50") + Text("vacation") + Pause("50") + Key("enter"),
    "label house": Pause("50") + Key("plus, l/50") + Text("house") + Pause("50") + Key("enter"),
    "this select": Key("plus, x"),
    "<n> select": Key("plus, x, plus, j") * Repeat(extra="n"),
    "(message|messages) reload": Key("plus, N"),
    "go inbox|going box": Key("plus, g, i"),
    "go starred": Key("plus, g, s"),
    "go sent": Key("plus, g, t"),
    "go drafts": Key("plus, g, d"),
    #"expand all": webdriver.ClickElementAction(By.XPATH, "//*[@aria-label='Expand all']"),
    #"collapse all": webdriver.ClickElementAction(By.XPATH, "//*[@aria-label='Collapse all']"),
    #"go field to": webdriver.ClickElementAction(By.XPATH, "//*[@aria-label='To']"),
    "go field cc": Key("cs-c"),
    "chat open": Key("plus, q"),
    "this send": Key("c-enter"),
    "go search": Key("plus, slash"),
  }

